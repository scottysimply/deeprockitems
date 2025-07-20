using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Content.Buffs
{
    public class TemperatureGlobalNPC : GlobalNPC
    {
        /// <summary>
        /// The temperature of this NPC. Will remain between <see cref="ColdThreshold"/> and <see cref="HeatThreshold"/>
        /// </summary>
        public int Temperature { get; set; }
        /// <summary>
        /// The time since this NPC's temperature has changed.
        /// </summary>
        public int TimeSinceTemperatureChanged { get; set; }
        /// <summary>
        /// The whoAmI of the last player to change this NPC's temperature.
        /// </summary>
        public int PlayerWhoLastChangedTemperature { get; set; }
        /// <summary>
        /// The cooldown before this NPC's temperature can change. Used after temperature shocking the enemy
        /// </summary>
        public int TempChangeCooldown { get; set; } = 0;
        public bool LastTileCollide { get; set; }
        private float _lastKBResist;

        #region Cold/Freeze variables
        public int ColdThreshold { get; private set; } = -100; // Default cold threshold
        /// <summary>
        /// The time (in ticks) that this enemy will be frozen for.
        /// </summary>
        public int TimeToStayFrozenFor { get; private set; } = 120;
        /// <summary>
        /// If true, this enemy will instead receive Frostburn while at minimum temperature
        /// </summary>
        public bool ImmuneToFreeze { get; set; } = false;
        public bool IsFrozen { get; set; } = false;
        private bool _oldFrozen = false;
        public bool FirstFrameFrozen { get => IsFrozen && !_oldFrozen; }

        private int _fallingTime = 0;
        private bool _isFalling = false;
        #endregion
        #region Heat/Fear variables
        public bool IsMarkedForDeath { get; set; } = false;
        /// <summary>
        /// If true, this enemy will instead receive On Fire! while at max temperature
        /// </summary>
        public bool ImmuneToMarkForDeath { get; set; } = false;
        /// <summary>
        /// The time (in ticks) that this enemy will be marked for death for.
        /// </summary>
        public int TimeToStayMarkedFor { get; private set; } = 120;
        private bool _oldMarked = false;
        public float DamageMultiplier = 1f;
        public bool FirstFrameMarked { get => IsMarkedForDeath && !_oldMarked; }
        public int HeatThreshold { get; private set; } = 100; // Default heat threshold
        #endregion
        public override bool InstancePerEntity => true;
        public override void ResetEffects(NPC npc)
        {
            _oldFrozen = IsFrozen;
            IsFrozen = false;
            _oldMarked = IsMarkedForDeath;
            IsMarkedForDeath = false;
        }
        public override void SetDefaults(NPC entity)
        {
            LastTileCollide = entity.noTileCollide;
            _lastKBResist = entity.knockBackResist;

            // Set temperature thresholds if enemy is immune to specific debuffs
            if (entity.buffImmune[BuffID.OnFire])
            {
                HeatThreshold *= 2;
            }
            if (entity.buffImmune[BuffID.Frostburn])
            {
                ColdThreshold *= 2;
            }
            // Add 0.1 x threshold if defense is higher than 15.
            if (entity.defense > 15)
            {
                HeatThreshold = (int)(HeatThreshold * (1f + 0.1f * (entity.defense - 15)));
                ColdThreshold = (int)(ColdThreshold * (1f + 0.1f * (entity.defense - 15)));
            }
            // Change time to stay frozen for based on knockback resist
            TimeToStayFrozenFor = (int)(30 + 0.75 * TimeToStayFrozenFor * entity.knockBackResist);
            // Make bosses immune to both marking and freeze
            if (entity.boss)
            {
                ImmuneToFreeze = true;
                ImmuneToMarkForDeath = true;
            }
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            // Set buff id
            int buffId = -1;
            if (Temperature <= this.ColdThreshold)
            {
                buffId = Main.hardMode ? BuffID.Frostburn2 : BuffID.Frostburn;
            }
            else if (Temperature >= this.HeatThreshold)
            {
                buffId = Main.hardMode ? BuffID.OnFire3 : BuffID.OnFire;
                /*if (!ImmuneToMarkForDeath)
                {
                    npc.AddBuff(ModContent.BuffType<FearingConfused>(), 2);
                }*/
            }
            // If buff id wasn't -1, apply the buff for 2 ticks
            if (buffId != -1)
            {
                npc.AddBuff(buffId, 2);
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (TempChangeCooldown > 0)
            {
                TempChangeCooldown--;
            }
            // Determine if NPC is too hot or too cold
            if (Temperature <= this.ColdThreshold)
            {
                if (!ImmuneToFreeze)
                {
                    // Freeze enemy
                    IsFrozen = true;
                }
            }
            else if (Temperature >= this.HeatThreshold)
            {
                if (!ImmuneToMarkForDeath)
                {
                    // Mark enemy for death
                    IsMarkedForDeath = true;
                }
            }

            // If temperature is not zero and enemy is not falling, increase time since temperature changed
            if (Temperature != 0 && !_isFalling)
            {
                TimeSinceTemperatureChanged++;
                // Test for cold temperature first.
                if (Temperature < 0)
                {
                    // If time since the last temperature change is above half the time to be frozen for
                    if (TimeSinceTemperatureChanged >= TimeToStayFrozenFor / 2)
                    {
                        // If unfrozen, make temperature go toward zero
                        if (Temperature > ColdThreshold)
                        {
                            Temperature++;
                        }
                        else if (TimeSinceTemperatureChanged >= TimeToStayFrozenFor) // If frozen, unfreeze
                        {
                            // Unfreeze
                            IsFrozen = false;
                            Temperature = 0;
                        }
                    }
                }
                else
                {
                    // If time since the last temperature change is above half the time to be marked for
                    if (TimeSinceTemperatureChanged >= TimeToStayMarkedFor / 2)
                    {
                        // If unmarked, make temperature go toward zero
                        if (Temperature < HeatThreshold)
                        {
                            Temperature--;
                        }
                        else if (TimeSinceTemperatureChanged >= TimeToStayMarkedFor) // If marked, unmark
                        {
                            IsMarkedForDeath = false;
                            Temperature = 0;
                        }
                    }
                }

                // If temperature is now zero, set time since temp changed to 0.
                if (Temperature == 0)
                {
                    TimeSinceTemperatureChanged = 0;
                }
            }

            // If an NPC is marked for death, increase damage taken.
            if (IsMarkedForDeath)
            {
                DamageMultiplier = 2f;
            }

            // If an NPC was just frozen, find the floor beneath them and determine if they are falling
            if (FirstFrameFrozen)
            {
                LastTileCollide = npc.noTileCollide;
                _lastKBResist = npc.knockBackResist;
                npc.noTileCollide = false;
                npc.knockBackResist = 0f;
                // Reset y velocity
                npc.velocity.Y = 0;
                for (int i = 0; i < 8; i++)
                {
                    // If npc is grounded
                    if (Collision.SolidTiles(new Vector2(npc.position.X, npc.position.Y + 2 * i), npc.width, npc.height))
                    {
                        // Grounded
                        _isFalling = false;
                        break;
                    }
                    _isFalling = true; // Set npc to falling
                }
                return base.PreAI(npc);
            }
            // If enemy is frozen (generally), make the NPC fall and inflict fall damage. Disable their velocities
            if (IsFrozen)
            {
                if (_isFalling)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        // Damage npc proportional to falling time
                        if (_fallingTime > 30)
                        {
                            var info = npc.CalculateHitInfo((int)(_fallingTime * 3f), 1);
                            npc.StrikeNPC(info);
                        }
                        _fallingTime = 0;
                        _isFalling = false;
                    }
                    else
                    {
                        // Increase falling time (height)
                        _fallingTime++;
                    }
                }
                // Set x velocity to zero
                npc.velocity.X = 0;
                // Apply gravity
                npc.velocity.Y += 0.5f;

                // Stop vanilla code
                return false;
            }
            else
            {
                npc.noTileCollide = LastTileCollide;
                npc.knockBackResist = _lastKBResist;
            }

            // Do vanilla PreAI code.
            return base.PreAI(npc);
        }
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (IsFrozen)
            {
                return false;
            }
            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
        public override bool CanHitNPC(NPC npc, NPC target)
        {
            if (IsFrozen)
            {
                return false;
            }
            return base.CanHitNPC(npc, target);
        }
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (IsFrozen && (modifiers.DamageType == DamageClass.Melee || modifiers.DamageType == DamageClass.Summon || modifiers.DamageType == DamageClass.MagicSummonHybrid || modifiers.DamageType == DamageClass.SummonMeleeSpeed))
            {
                modifiers.SourceDamage *= 2f;
                modifiers.HideCombatText();
            }
            else if (IsMarkedForDeath && (modifiers.DamageType == DamageClass.Ranged || modifiers.DamageType == DamageClass.Magic || modifiers.DamageType == DamageClass.MagicSummonHybrid))
            {
                modifiers.SourceDamage *= 2f;
                modifiers.HideCombatText();
            }
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
            if (IsFrozen && (hit.DamageType == DamageClass.Melee || hit.DamageType == DamageClass.Summon || hit.DamageType == DamageClass.MagicSummonHybrid || hit.DamageType == DamageClass.SummonMeleeSpeed))
            {
                CombatText.NewText(npc.getRect(), Frozen_Text_Color, damageDone);
            }
            else if (IsMarkedForDeath && (hit.DamageType == DamageClass.Ranged || hit.DamageType == DamageClass.Magic || hit.DamageType == DamageClass.MagicSummonHybrid))
            {
                CombatText.NewText(npc.getRect(), Marked_Text_Color, damageDone);
            }
        }
        Color Frozen_Text_Color => new Color(110, 150, 245);
        Color Marked_Text_Color => new Color(245, 30, 60);
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
            if (IsFrozen && (hit.DamageType == DamageClass.Melee || hit.DamageType == DamageClass.Summon || hit.DamageType == DamageClass.MagicSummonHybrid || hit.DamageType == DamageClass.SummonMeleeSpeed))
            {
                CombatText.NewText(npc.getRect(), Frozen_Text_Color, damageDone);
            }
            else if (IsMarkedForDeath && (hit.DamageType == DamageClass.Ranged || hit.DamageType == DamageClass.Magic || hit.DamageType == DamageClass.MagicSummonHybrid))
            {
                CombatText.NewText(npc.getRect(), Marked_Text_Color, damageDone);
            }
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (IsFrozen)
            {
                drawColor = Lighting.GetColor(npc.Center.ToTileCoordinates(), new Color(125, 175, 240));
            }
            else if (IsMarkedForDeath)
            {
                drawColor = Lighting.GetColor(npc.Center.ToTileCoordinates(), new Color(196, 43, 26));
            }
            else
            {
                base.DrawEffects(npc, ref drawColor);
            }
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }
    }
    public static class FrozenNPCExtension
    {
        /// <summary>
        /// Modifies the NPC's <see cref="TemperatureGlobalNPC.Temperature">Temperature</see> and sets relevant properties/fields. Should not be set from the GlobalNPC.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="amountToChangeBy"></param>
        public static void ChangeTemperature(this NPC self, int amountToAdd, int player = -1)
        {
            // Check real life prior to doing anything
            int realID = self.realLife > -1 ? self.realLife : self.whoAmI;
            var modNPC = Main.npc[realID].GetGlobalNPC<TemperatureGlobalNPC>();
            if (modNPC.TempChangeCooldown > 0) return;
            if (modNPC.Temperature + amountToAdd >= modNPC.HeatThreshold) // Too high
            {
                modNPC.Temperature = modNPC.HeatThreshold;
            }
            else if (modNPC.Temperature + amountToAdd <= modNPC.ColdThreshold) // Too low
            {
                modNPC.Temperature = modNPC.ColdThreshold;
            }
            else // Normal case
            {
                // If frozen or on fire, temp shock
                int newAmountToAdd = amountToAdd;
                if (modNPC.IsFrozen || modNPC.IsMarkedForDeath)
                {
                    self.TempShock(ref newAmountToAdd, player);
                }
                modNPC.Temperature += newAmountToAdd;
            }
            modNPC.PlayerWhoLastChangedTemperature = player;
            modNPC.TimeSinceTemperatureChanged = 0;
        }
        /// <summary>
        /// Deals temperature shock to an NPC, and sets temperature accordingly. The ref value is the amount of temperature added.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="finalTemperatureAdded"></param>
        /// <param name="player"></param>
        public static void TempShock(this NPC self, ref int tempAmount, int player = -1)
        {
            var info = self.CalculateHitInfo(3 * Math.Abs(tempAmount), -1, damageType: DamageClass.Generic, damageVariation: true);
            if (player != -1)
            {
                Main.player[player].StrikeNPCDirect(self, info);
                tempAmount = 0;
            }
            else
            {
                self.StrikeNPC(info);
            }
            var modNPC = self.GetGlobalNPC<TemperatureGlobalNPC>();
            modNPC.TempChangeCooldown = 15;
            modNPC.Temperature = 0;
            
        }
        /// <summary>
        /// Gets the temperature of an NPC, which ranges from -100 to 100
        /// </summary>
        public static int GetTemperature(this NPC self) => self.GetGlobalNPC<TemperatureGlobalNPC>().Temperature;
    }
}
