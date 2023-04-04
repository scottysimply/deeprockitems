using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace deeprockitems.Assets.Text
{
    public static class LaserText
    {
        static readonly Color TeamRed = new(.9f, .3f, .3f, .85f); // Red
        static readonly Color TeamGreen = new(.3f, .9f, .3f, .85f); // Green
        static readonly Color TeamBlue = new(.4f, .8f, .8f, .8f); // Blue
        static readonly Color TeamYellow = new(.8f, .8f, .25f, .8f); // Yellow
        static readonly Color TeamPink = new(.8f, .25f, .75f, .8f); // Pink
        static readonly Color TeamNone = new(.95f, .95f, .95f, .85f); // No team
        public static Color GetColor(Player player)
        {
            return player.team switch
            {
                1 => TeamRed,
                2 => TeamGreen,
                3 => TeamBlue,
                4 => TeamYellow,
                5 => TeamPink,
                _ => TeamNone
            };
        }
        public static string TileBark(Tile TileTarget, Player SelfPlayer)
        {
            string Bark = Main.rand.Next(0, 9) switch
            {
                0 => "Found something!",
                1 => "Over here!",
                2 => "What is that?",
                3 => "Is it drillable?",
                4 => "Maarrk!",
                5 => "Look at this!",
                6 => "Marker up!",
                7 => "Guys, look over here!",
                8 => "Attention!",
                _ => "I've never seen that before!"
            };

            return Bark;
        }
        public static string NPCBark(NPC NPCTarget, Player SelfPlayer)
        {
            string Bark = "";
            if (!NPCTarget.friendly)
            {
                if (SelfPlayer.numMinions > 0)
                {
                    Bark = Main.rand.Next(0, 9) switch
                    {
                        0 => "Attack, fire, fire, shoot!",
                        1 => "Target that enemy!",
                        2 => "Right here!",
                        3 => "Shoot it!",
                        4 => "Kill it!",
                        5 => "Exterminate. Exterminate!",
                        6 => "Aim here!",
                        7 => "There's our problem!",
                        8 => "Attack this enemy!",
                        _ => "Minion attack voiceline"
                    };
                }
            }
            else
            {
                if (NPCTarget.type == NPCID.Guide)
                {
                    Bark = Main.rand.Next(0, 4) switch
                    {
                        0 => "Hey. Guide. I need help!",
                        1 => "I don't trust this one",
                        2 => "Do you smell paint?",
                        3 => "",
                        
                        _ => "GUIDE_TEXT"
                    };
                }
            }



            return Bark;
        }
        public static string PlayerBark(Player TargetPlayer, Player SelfPlayer)
        {
            string Bark = "";
            if (SelfPlayer.hostile && TargetPlayer.hostile) // If either player has PVP on, taunt.
            {
                Bark = Main.rand.Next(0, 9) switch
                {
                    0 => "It's you and me, knife-ears!",
                    1 => "You're going down!",
                    2 => "I dislike your style, miner.",
                    3 => "You're as tough as talc?",
                    4 => "Should we talk this one out?",
                    5 => "Let's do this, cobalt-breath",
                    6 => "Are your weapons made of chalk?",
                    7 => "My wife has a beard twice the size of yours!",
                    8 => "Is that an axe I see?",
                    _ => "I'm undefeated! I've also never fought anyone..."
                };
            }
            else if ((TargetPlayer.statLife / TargetPlayer.statLifeMax) < .1) // Else if player on relatively low health, mention health
            {

            }
            else // No special cases, say random line
            {

            }

            return Bark;
        }
    }
}
