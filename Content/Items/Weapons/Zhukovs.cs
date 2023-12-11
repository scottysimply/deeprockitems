using deeprockitems.Common.Weapons;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.Player;

namespace deeprockitems.Content.Items.Weapons
{
    public class Zhukovs : UpgradeableItemTemplate
    {
        public override void SafeDefaults()
        {
            Item.width = 52;
            Item.height = 46;
            Item.rare = ItemRarityID.Cyan;

            Item.damage = 12;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.crit = 46;
            Item.knockBack = 0.4f;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 5;
            Item.useAmmo = AmmoID.Bullet;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.channel = true;



            Item.value = Item.sellPrice(0, 6, 50, 0);

        }
        /*
         * if (num == ModContent.ItemType<deeprockitems.Content.Items.Weapons.Zhukovs>())
				{
					Vector2 textureCenter = new Vector2(itemDrawFrame.Width / 2f, itemDrawFrame.Height / 2f);
					Vector2 drawOffset = new Vector2(0f, 0f);
					int drawOffX = (int)drawOffset.X;
					textureCenter.Y = drawOffset.Y;
					Vector2 drawOrigin = new Vector2(-1f * (float)drawOffX, itemDrawFrame.Height / 2f);
					if (drawinfo.drawPlayer.direction == -1)
					{
						drawOrigin = new Vecotr2(itemDrawFrame.Width + drawOffX, itemDrawFrame.Height / 2f);
					}
					Texture2D zhukovsSprite;
					try
					{
						zhukovsSprite = ModContent.Request<Texture2D>("deeprockitems/Content/Items/Weapons/ZhukovsHeld");
					}
					catch (Terraria.ModLoader.Exceptions.MissingResourceException e)
					{
						Mod drg = ModContent.GetInstance<deeprockitems>();
						drg.Logger.Warn("Unable to retrieve Zhukovs' held item texture. Zhukovs will not draw properly.");
					}
					// Offhand first since it's in the back.
					item = new DrawData(zhukovsSprite, new Vector2(drawinfo.ItemLocation.X - Main.screenPosition.X + textureCenter.X, drawinfo.ItemLocation.Y - Main.screenPosition.Y + textureCenter.Y), new Rectangle?(itemDrawFrame), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, drawOrigin, adjustedItemScale, drawinfo.itemEffect, 0f);
					drawinfo.DrawDataCache.Add(item);

					// Mainhand in the front.
					item = new DrawData(zhukovsSprite, new Vector2(drawinfo.ItemLocation.X - Main.screenPosition.X + textureCenter.X, drawinfo.ItemLocation.Y - Main.screenPosition.Y + textureCenter.Y), new Rectangle?(itemDrawFrame), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, drawOrigin, adjustedItemScale, drawinfo.itemEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
				}
        */
        private int _originalProjectile;
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            _originalProjectile = type;
            type = ModContent.ProjectileType<Projectiles.ZhukovProjectile.ZhukovHelper>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback);
            if (proj.ModProjectile is Projectiles.ZhukovProjectile.ZhukovHelper modProj)
            {
                modProj.ProjectileToSpawn = _originalProjectile;
                // Sorry, until this weird gravity issue gets fixed: No modded bullets!
                if (!ModInformation.IsProjectileVanilla(_originalProjectile) && !ModInformation.IsProjectileMyMod(_originalProjectile))
                {
                    modProj.ProjectileToSpawn = ProjectileID.BulletHighVelocity;
                }
            }
            return false;
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {

        }
        public override void UseAnimation(Player player)
        {

        }
    }
    public class ILDetourTest : ModSystem
    {
        public override void Load()
        {
            On_PlayerDrawLayers.DrawPlayer_27_HeldItem += On_PlayerDrawLayers_DrawPlayer_27_HeldItem;
        }

        private void On_PlayerDrawLayers_DrawPlayer_27_HeldItem(On_PlayerDrawLayers.orig_DrawPlayer_27_HeldItem orig, ref PlayerDrawSet drawinfo)
        {
            if (drawinfo.drawPlayer.JustDroppedAnItem)
            {
                return;
            }
            if (drawinfo.drawPlayer.heldProj >= 0 && drawinfo.shadow == 0f && !drawinfo.heldProjOverHand)
            {
                drawinfo.projectileDrawPosition = drawinfo.DrawDataCache.Count;
            }
            Item heldItem = drawinfo.heldItem;
            int num = heldItem.type;
            if (drawinfo.drawPlayer.UsingBiomeTorches)
            {
                if (num != 8)
                {
                    if (num == 966)
                    {
                        num = drawinfo.drawPlayer.BiomeCampfireHoldStyle(num);
                    }
                }
                else
                {
                    num = drawinfo.drawPlayer.BiomeTorchHoldStyle(num);
                }
            }
            float adjustedItemScale = drawinfo.drawPlayer.GetAdjustedItemScale(heldItem);
            Main.instance.LoadItem(num);
            Texture2D value = TextureAssets.Item[num].Value;
            Vector2 position;
            position = new((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y)));
            Rectangle itemDrawFrame = drawinfo.drawPlayer.GetItemDrawFrame(num);
            drawinfo.itemColor = Lighting.GetColor((int)((double)drawinfo.Position.X + (double)drawinfo.drawPlayer.width * 0.5) / 16, (int)(((double)drawinfo.Position.Y + (double)drawinfo.drawPlayer.height * 0.5) / 16.0));
            if (num == 678)
            {
                drawinfo.itemColor = Color.White;
            }
            if (drawinfo.drawPlayer.shroomiteStealth && heldItem.DamageType == DamageClass.Ranged)
            {
                float num2 = drawinfo.drawPlayer.stealth;
                if ((double)num2 < 0.03)
                {
                    num2 = 0.03f;
                }
                float num3 = (1f + num2 * 10f) / 11f;
                drawinfo.itemColor = new Color((int)((byte)((float)drawinfo.itemColor.R * num2)), (int)((byte)((float)drawinfo.itemColor.G * num2)), (int)((byte)((float)drawinfo.itemColor.B * num3)), (int)((byte)((float)drawinfo.itemColor.A * num2)));
            }
            if (drawinfo.drawPlayer.setVortex && heldItem.DamageType == DamageClass.Ranged)
            {
                float num4 = drawinfo.drawPlayer.stealth;
                if ((double)num4 < 0.03)
                {
                    num4 = 0.03f;
                }
                float num20 = (1f + num4 * 10f) / 11f;
                drawinfo.itemColor = drawinfo.itemColor.MultiplyRGBA(new Color(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num4)));
            }
            bool flag = drawinfo.drawPlayer.itemAnimation > 0 && heldItem.useStyle != 0;
            bool flag2 = heldItem.holdStyle != 0 && !drawinfo.drawPlayer.pulley;
            if (!drawinfo.drawPlayer.CanVisuallyHoldItem(heldItem))
            {
                flag2 = false;
            }
            if (drawinfo.shadow != 0f || drawinfo.drawPlayer.frozen || (!flag && !flag2) || num <= 0 || drawinfo.drawPlayer.dead || heldItem.noUseGraphic || (drawinfo.drawPlayer.wet && heldItem.noWet && !ItemID.Sets.WaterTorches[num]) || (drawinfo.drawPlayer.happyFunTorchTime && drawinfo.drawPlayer.inventory[drawinfo.drawPlayer.selectedItem].createTile == 4 && drawinfo.drawPlayer.itemAnimation == 0))
            {
                return;
            }
            string name = drawinfo.drawPlayer.name;
            Color color;
            color = new(250, 250, 250, heldItem.alpha);
            Vector2 vector = Vector2.Zero;
            if (num <= 426)
            {
                if (num <= 104)
                {
                    if (num == 46)
                    {
                        float amount = Utils.Remap(drawinfo.itemColor.ToVector3().Length() / 1.731f, 0.3f, 0.5f, 1f, 0f, true);
                        color = Color.Lerp(Color.Transparent, new Color(255, 255, 255, 127) * 0.7f, amount);
                        goto IL_50C;
                    }
                    if (num != 104)
                    {
                        goto IL_50C;
                    }
                }
                else
                {
                    if (num == 204)
                    {
                        vector = new Vector2(4f, -6f) * drawinfo.drawPlayer.Directions;
                        goto IL_50C;
                    }
                    if (num != 426)
                    {
                        goto IL_50C;
                    }
                    goto IL_43C;
                }
            }
            else if (num <= 1506)
            {
                if (num != 797 && num != 1506)
                {
                    goto IL_50C;
                }
                goto IL_43C;
            }
            else
            {
                if (num == 3349)
                {
                    vector = new Vector2(2f, -2f) * drawinfo.drawPlayer.Directions;
                    goto IL_50C;
                }
                if (num - 5094 > 1)
                {
                    if (num - 5096 > 1)
                    {
                        goto IL_50C;
                    }
                    goto IL_43C;
                }
            }
            vector = new Vector2(4f, -4f) * drawinfo.drawPlayer.Directions;
            goto IL_50C;
        IL_43C:
            vector = new Vector2(6f, -6f) * drawinfo.drawPlayer.Directions;
        IL_50C:
            if (num == 3823)
            {
                vector = new((float)(7 * drawinfo.drawPlayer.direction), -7f * drawinfo.drawPlayer.gravDir);
            }
            if (num == 3827)
            {
                vector = new((float)(13 * drawinfo.drawPlayer.direction), -13f * drawinfo.drawPlayer.gravDir);
                color = heldItem.GetAlpha(drawinfo.itemColor);
                color = Color.Lerp(color, Color.White, 0.6f);
                color.A = 66;
            }
            Vector2 origin;
            origin = new((float)itemDrawFrame.Width * 0.5f - (float)itemDrawFrame.Width * 0.5f * (float)drawinfo.drawPlayer.direction, (float)itemDrawFrame.Height);
            if (heldItem.useStyle == 9 && drawinfo.drawPlayer.itemAnimation > 0)
            {
                Vector2 vector2;
                vector2 = new(0.5f, 0.4f);
                if (heldItem.type == 5009 || heldItem.type == 5042)
                {
                    vector2 = new(0.26f, 0.5f);
                    if (drawinfo.drawPlayer.direction == -1)
                    {
                        vector2.X = 1f - vector2.X;
                    }
                }
                origin = itemDrawFrame.Size() * vector2;
            }
            if (drawinfo.drawPlayer.gravDir == -1f)
            {
                origin.Y = (float)itemDrawFrame.Height - origin.Y;
            }
            origin += vector;
            float num5 = drawinfo.drawPlayer.itemRotation;
            if (heldItem.useStyle == 8)
            {
                float num6 = position.X;
                int direction = drawinfo.drawPlayer.direction;
                position.X = num6 - 0f;
                num5 -= 1.5707964f * (float)drawinfo.drawPlayer.direction;
                origin.Y = 2f;
                origin.X += (float)(2 * drawinfo.drawPlayer.direction);
            }
            if (num == 425 || num == 507)
            {
                if (drawinfo.drawPlayer.gravDir == 1f)
                {
                    if (drawinfo.drawPlayer.direction == 1)
                    {
                        drawinfo.itemEffect = (SpriteEffects)2;
                    }
                    else
                    {
                        drawinfo.itemEffect = (SpriteEffects)3;
                    }
                }
                else if (drawinfo.drawPlayer.direction == 1)
                {
                    drawinfo.itemEffect = 0;
                }
                else
                {
                    drawinfo.itemEffect = (SpriteEffects)1;
                }
            }
            if ((num == 946 || num == 4707) && num5 != 0f)
            {
                position.Y -= 22f * drawinfo.drawPlayer.gravDir;
                num5 = -1.57f * (float)(-(float)drawinfo.drawPlayer.direction) * drawinfo.drawPlayer.gravDir;
            }
            ItemSlot.GetItemLight(ref drawinfo.itemColor, heldItem, false);
            if (num == 3476)
            {
                Texture2D value2 = TextureAssets.Extra[64].Value;
                Rectangle rectangle2 = value2.Frame(1, 9, 0, drawinfo.drawPlayer.miscCounter % 54 / 6, 0, 0);
                Vector2 vector3;
                vector3 = new((float)(rectangle2.Width / 2 * drawinfo.drawPlayer.direction), 0f);
                Vector2 origin2 = rectangle2.Size() / 2f;
                DrawData item = new DrawData(value2, (drawinfo.ItemLocation - Main.screenPosition + vector3).Floor(), new Rectangle?(rectangle2), heldItem.GetAlpha(drawinfo.itemColor).MultiplyRGBA(new Color(new Vector4(0.5f, 0.5f, 0.5f, 0.8f))), drawinfo.drawPlayer.itemRotation, origin2, adjustedItemScale, drawinfo.itemEffect, 0f);
                drawinfo.DrawDataCache.Add(item);
                value2 = TextureAssets.GlowMask[195].Value;
                item = new DrawData(value2, (drawinfo.ItemLocation - Main.screenPosition + vector3).Floor(), new Rectangle?(rectangle2), new Color(250, 250, 250, heldItem.alpha) * 0.5f, drawinfo.drawPlayer.itemRotation, origin2, adjustedItemScale, drawinfo.itemEffect, 0f);
                drawinfo.DrawDataCache.Add(item);
                return;
            }
            if (num == 3779)
            {
                Texture2D texture2D = value;
                Rectangle rectangle3 = texture2D.Frame(1, 1, 0, 0, 0, 0);
                Vector2 vector4;
                vector4 = new((float)(rectangle3.Width / 2 * drawinfo.drawPlayer.direction), 0f);
                Vector2 origin3 = rectangle3.Size() / 2f;
                float num7 = ((float)drawinfo.drawPlayer.miscCounter / 75f * 6.2831855f).ToRotationVector2().X * 1f + 0f;
                Color color2 = new Color(120, 40, 222, 0) * (num7 / 2f * 0.3f + 0.85f) * 0.5f;
                num7 = 2f;
                DrawData item;
                for (float num8 = 0f; num8 < 4f; num8 += 1f)
                {
                    item = new DrawData(TextureAssets.GlowMask[218].Value, (drawinfo.ItemLocation - Main.screenPosition + vector4).Floor() + (num8 * 1.5707964f).ToRotationVector2() * num7, new Rectangle?(rectangle3), color2, drawinfo.drawPlayer.itemRotation, origin3, adjustedItemScale, drawinfo.itemEffect, 0f);
                    drawinfo.DrawDataCache.Add(item);
                }
                item = new DrawData(texture2D, (drawinfo.ItemLocation - Main.screenPosition + vector4).Floor(), new Rectangle?(rectangle3), heldItem.GetAlpha(drawinfo.itemColor).MultiplyRGBA(new Color(new Vector4(0.5f, 0.5f, 0.5f, 0.8f))), drawinfo.drawPlayer.itemRotation, origin3, adjustedItemScale, drawinfo.itemEffect, 0f);
                drawinfo.DrawDataCache.Add(item);
                return;
            }
            if (num == 4049)
            {
                Texture2D value3 = TextureAssets.Extra[92].Value;
                Rectangle rectangle4 = value3.Frame(1, 4, 0, drawinfo.drawPlayer.miscCounter % 20 / 5, 0, 0);
                Vector2 vector5;
                vector5 = new((float)(rectangle4.Width / 2 * drawinfo.drawPlayer.direction), 0f);
                vector5 += new Vector2((float)(-10 * drawinfo.drawPlayer.direction), 8f * drawinfo.drawPlayer.gravDir);
                Vector2 origin4 = rectangle4.Size() / 2f;
                DrawData item = new DrawData(value3, (drawinfo.ItemLocation - Main.screenPosition + vector5).Floor(), new Rectangle?(rectangle4), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin4, adjustedItemScale, drawinfo.itemEffect, 0f);
                drawinfo.DrawDataCache.Add(item);
                return;
            }
            if (heldItem.useStyle == 5)
            {
                DrawData item;
                if (Item.staff[num])
                {
                    float num9 = drawinfo.drawPlayer.itemRotation + 0.785f * (float)drawinfo.drawPlayer.direction;
                    float num10 = 0f;
                    float num11 = 0f;
                    Vector2 origin5;
                    origin5 = new(0f, (float)itemDrawFrame.Height);
                    if (num == 3210)
                    {
                        num10 = (float)(8 * -(float)drawinfo.drawPlayer.direction);
                        num11 = (float)(2 * (int)drawinfo.drawPlayer.gravDir);
                    }
                    if (num == 3870)
                    {
                        Vector2 vector9 = (drawinfo.drawPlayer.itemRotation + 0.7853982f * (float)drawinfo.drawPlayer.direction).ToRotationVector2() * new Vector2((float)(-(float)drawinfo.drawPlayer.direction) * 1.5f, drawinfo.drawPlayer.gravDir) * 3f;
                        num10 = (float)((int)vector9.X);
                        num11 = (float)((int)vector9.Y);
                    }
                    if (num == 3787)
                    {
                        num11 = (float)((int)((float)(8 * (int)drawinfo.drawPlayer.gravDir) * (float)Math.Cos((double)num9)));
                    }
                    if (num == 3209)
                    {
                        Vector2 vector10 = (new Vector2(-8f, 0f) * drawinfo.drawPlayer.Directions).RotatedBy((double)drawinfo.drawPlayer.itemRotation, default(Vector2));
                        num10 = vector10.X;
                        num11 = vector10.Y;
                    }
                    if (drawinfo.drawPlayer.gravDir == -1f)
                    {
                        if (drawinfo.drawPlayer.direction == -1)
                        {
                            num9 += 1.57f;
                            origin5 = new((float)itemDrawFrame.Width, 0f);
                            num10 -= (float)itemDrawFrame.Width;
                        }
                        else
                        {
                            num9 -= 1.57f;
                            origin5 = Vector2.Zero;
                        }
                    }
                    else if (drawinfo.drawPlayer.direction == -1)
                    {
                        origin5 = new((float)itemDrawFrame.Width, (float)itemDrawFrame.Height);
                        num10 -= (float)itemDrawFrame.Width;
                    }
                    ItemLoader.HoldoutOrigin(drawinfo.drawPlayer, ref origin5);
                    item = new DrawData(value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + origin5.X + num10)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num11))), new Rectangle?(itemDrawFrame), heldItem.GetAlpha(drawinfo.itemColor), num9, origin5, adjustedItemScale, drawinfo.itemEffect, 0f);
                    drawinfo.DrawDataCache.Add(item);
                    if (num == 3870)
                    {
                        item = new DrawData(TextureAssets.GlowMask[238].Value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + origin5.X + num10)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num11))), new Rectangle?(itemDrawFrame), new Color(255, 255, 255, 127), num9, origin5, adjustedItemScale, drawinfo.itemEffect, 0f);
                        drawinfo.DrawDataCache.Add(item);
                    }
                    return;
                }
                if (num == 5118)
                {
                    float rotation = drawinfo.drawPlayer.itemRotation + 1.57f * (float)drawinfo.drawPlayer.direction;
                    Vector2 vector6;
                    vector6 = new((float)itemDrawFrame.Width * 0.5f, (float)itemDrawFrame.Height * 0.5f);
                    Vector2 origin6;
                    origin6 = new((float)itemDrawFrame.Width * 0.5f, (float)itemDrawFrame.Height);
                    Vector2 spinningpoint = new Vector2(10f, 4f) * drawinfo.drawPlayer.Directions;
                    spinningpoint = spinningpoint.RotatedBy((double)drawinfo.drawPlayer.itemRotation, default(Vector2));
                    item = new DrawData(value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector6.X + spinningpoint.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector6.Y + spinningpoint.Y))), new Rectangle?(itemDrawFrame), heldItem.GetAlpha(drawinfo.itemColor), rotation, origin6, adjustedItemScale, drawinfo.itemEffect, 0f);
                    drawinfo.DrawDataCache.Add(item);
                    return;
                }
                // MY CODE
                        if (num == ModContent.ItemType<Zhukovs>())
                        {
                            Vector2 textureCenter = new Vector2(itemDrawFrame.Width / 2f, itemDrawFrame.Height / 2f);
                            Vector2 drawOffset = new Vector2(0f, 16f * drawinfo.drawPlayer.gravDir);
                            int drawOffX = (int)drawOffset.X;
                            textureCenter.Y = drawOffset.Y;
                            Vector2 drawOrigin = new Vector2(-1f * (float)drawOffX, itemDrawFrame.Height / 2f);
                            if (drawinfo.drawPlayer.direction == -1)
                            {
                                drawOrigin = new Vector2(itemDrawFrame.Width + drawOffX, itemDrawFrame.Height / 2f);
                            }
                            Texture2D zhukovsSprite;
                            try
                            {
                                zhukovsSprite = (Texture2D)ModContent.Request<Texture2D>("deeprockitems/Content/Items/Weapons/ZhukovsHeld");
                            }
                            catch
                            {
                                Mod drg = ModContent.GetInstance<deeprockitems>();
                                drg.Logger.Warn("Unable to retrieve Zhukovs' held item texture. Zhukovs will not draw properly.");
                                return;
                            }

                            DualWieldPlayer modPlayer = drawinfo.drawPlayer.GetModPlayer<DualWieldPlayer>();

                            float offhandScale = 0.8f;
                            // Offhand first since it's in the back.
                            item = new DrawData(zhukovsSprite, new Vector2(modPlayer.offHandItemLocation.X - Main.screenPosition.X + textureCenter.X, modPlayer.offHandItemLocation.Y - Main.screenPosition.Y + textureCenter.Y), new Rectangle?(itemDrawFrame), heldItem.GetAlpha(drawinfo.itemColor), modPlayer.offHandItemRotation, drawOrigin, adjustedItemScale * offhandScale, drawinfo.itemEffect, 0f);
                            drawinfo.DrawDataCache.Add(item);

                            // Mainhand in the front.
                            item = new DrawData(zhukovsSprite, new Vector2(drawinfo.ItemLocation.X - Main.screenPosition.X + textureCenter.X, drawinfo.ItemLocation.Y - Main.screenPosition.Y + textureCenter.Y), new Rectangle?(itemDrawFrame), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, drawOrigin, adjustedItemScale, drawinfo.itemEffect, 0f);
                            drawinfo.DrawDataCache.Add(item);
                    return;
                        }
                Vector2 vector7;
                vector7 = new((float)(itemDrawFrame.Width / 2), (float)(itemDrawFrame.Height / 2));
                Vector2 vector8 = Main.DrawPlayerItemPos(drawinfo.drawPlayer.gravDir, num);
                int num12 = (int)vector8.X;
                vector7.Y = vector8.Y;
                Vector2 origin7;
                origin7 = new((float)(-(float)num12), (float)(itemDrawFrame.Height / 2));
                if (drawinfo.drawPlayer.direction == -1)
                {
                    origin7 = new((float)(itemDrawFrame.Width + num12), (float)(itemDrawFrame.Height / 2));
                }
                item = new DrawData(value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector7.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector7.Y))), new Rectangle?(itemDrawFrame), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin7, adjustedItemScale, drawinfo.itemEffect, 0f);
                drawinfo.DrawDataCache.Add(item);
                if (heldItem.color != default(Color))
                {
                    item = new DrawData(value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector7.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector7.Y))), new Rectangle?(itemDrawFrame), heldItem.GetColor(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin7, adjustedItemScale, drawinfo.itemEffect, 0f);
                    drawinfo.DrawDataCache.Add(item);
                }
                if (heldItem.glowMask != -1)
                {
                    item = new DrawData(TextureAssets.GlowMask[(int)heldItem.glowMask].Value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector7.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector7.Y))), new Rectangle?(itemDrawFrame), new Color(250, 250, 250, heldItem.alpha), drawinfo.drawPlayer.itemRotation, origin7, adjustedItemScale, drawinfo.itemEffect, 0f);
                    drawinfo.DrawDataCache.Add(item);
                }
                if (num == 3788)
                {
                    float num13 = ((float)drawinfo.drawPlayer.miscCounter / 75f * 6.2831855f).ToRotationVector2().X * 1f + 0f;
                    Color color3 = new Color(80, 40, 252, 0) * (num13 / 2f * 0.3f + 0.85f) * 0.5f;
                    for (float num14 = 0f; num14 < 4f; num14 += 1f)
                    {
                        item = new DrawData(TextureAssets.GlowMask[220].Value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector7.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector7.Y))) + (num14 * 1.5707964f + drawinfo.drawPlayer.itemRotation).ToRotationVector2() * num13, null, color3, drawinfo.drawPlayer.itemRotation, origin7, adjustedItemScale, drawinfo.itemEffect, 0f);
                        drawinfo.DrawDataCache.Add(item);
                    }
                }
                return;
            }
            else
            {
                DrawData item;
                if (drawinfo.drawPlayer.gravDir == -1f)
                {
                    item = new DrawData(value, position, new Rectangle?(itemDrawFrame), heldItem.GetAlpha(drawinfo.itemColor), num5, origin, adjustedItemScale, drawinfo.itemEffect, 0f);
                    drawinfo.DrawDataCache.Add(item);
                    if (heldItem.color != default(Color))
                    {
                        item = new DrawData(value, position, new Rectangle?(itemDrawFrame), heldItem.GetColor(drawinfo.itemColor), num5, origin, adjustedItemScale, drawinfo.itemEffect, 0f);
                        drawinfo.DrawDataCache.Add(item);
                    }
                    if (heldItem.glowMask != -1)
                    {
                        item = new DrawData(TextureAssets.GlowMask[(int)heldItem.glowMask].Value, position, new Rectangle?(itemDrawFrame), new Color(250, 250, 250, heldItem.alpha), num5, origin, adjustedItemScale, drawinfo.itemEffect, 0f);
                        drawinfo.DrawDataCache.Add(item);
                    }
                    return;
                }
                item = new DrawData(value, position, new Rectangle?(itemDrawFrame), heldItem.GetAlpha(drawinfo.itemColor), num5, origin, adjustedItemScale, drawinfo.itemEffect, 0f);
                drawinfo.DrawDataCache.Add(item);
                if (heldItem.color != default(Color))
                {
                    item = new DrawData(value, position, new Rectangle?(itemDrawFrame), heldItem.GetColor(drawinfo.itemColor), num5, origin, adjustedItemScale, drawinfo.itemEffect, 0f);
                    drawinfo.DrawDataCache.Add(item);
                }
                if (heldItem.glowMask != -1)
                {
                    item = new DrawData(TextureAssets.GlowMask[(int)heldItem.glowMask].Value, position, new Rectangle?(itemDrawFrame), color, num5, origin, adjustedItemScale, drawinfo.itemEffect, 0f);
                    drawinfo.DrawDataCache.Add(item);
                }
                if (!heldItem.flame || drawinfo.shadow != 0f)
                {
                    return;
                }
                try
                {
                    Main.instance.LoadItemFlames(num);
                    if (TextureAssets.ItemFlame[num].IsLoaded)
                    {
                        Color color4;
                        color4 = new(100, 100, 100, 0);
                        int num15 = 7;
                        float num16 = 1f;
                        float num17 = 0f;
                        if (num <= 4952)
                        {
                            if (num != 3045)
                            {
                                if (num == 4952)
                                {
                                    num15 = 3;
                                    num16 = 0.6f;
                                    color4 = new(50, 50, 50, 0);
                                }
                            }
                            else
                            {
                                color4 = new(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
                            }
                        }
                        else if (num != 5293)
                        {
                            if (num != 5322)
                            {
                                if (num == 5353)
                                {
                                    color4 = new(255, 255, 255, 200);
                                }
                            }
                            else
                            {
                                color4 = new(100, 100, 100, 150);
                                num17 = (float)(-2 * drawinfo.drawPlayer.direction);
                            }
                        }
                        else
                        {
                            color4 = new(50, 50, 100, 20);
                        }
                        for (int i = 0; i < num15; i++)
                        {
                            float num18 = drawinfo.drawPlayer.itemFlamePos[i].X * adjustedItemScale * num16;
                            float num19 = drawinfo.drawPlayer.itemFlamePos[i].Y * adjustedItemScale * num16;
                            item = new DrawData(TextureAssets.ItemFlame[num].Value, new Vector2((float)((int)(position.X + num18 + num17)), (float)((int)(position.Y + num19))), new Rectangle?(itemDrawFrame), color4, num5, origin, adjustedItemScale, drawinfo.itemEffect, 0f);
                            drawinfo.DrawDataCache.Add(item);
                        }
                    }
                }
                catch
                {
                }
                return;
            }
        }
    }
}
