using deeprockitems.Content.Buffs;
using deeprockitems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace deeprockitems.Content.Projectiles
{
    public class PrimitiveProjectileHandler : ModSystem
    {
        public class ProjectileVertex {
            /// <summary>
            /// Whether this vertex is an active instance.
            /// </summary>
            public bool Active;
            /// <summary>
            /// The list of connected vertices
            /// </summary>
            public bool[] ConnectedVertices;
            /// <summary>
            /// The identity of this projectile in the vertex array
            /// </summary>
            public int WhoAmI;
            /// <summary>
            /// The type this vertex will have (for merging)
            /// </summary>
            public int Type;
            /// <summary>
            /// The position of this vertex
            /// </summary>
            public Vector2 Position;
            /// <summary>
            /// The velocity of this vertex
            /// </summary>
            public Vector2 Velocity;
            /// <summary>
            /// How far a vertex will seek neighbors, in pixels
            /// </summary>
            public float Weight;
            /// <summary>
            /// Time in ticks before this vertex will die
            /// </summary>
            public int TimeLeft;
        }
        public ProjectileVertex[] Vertices;
        public override void SetStaticDefaults() {
            Vertices = new ProjectileVertex[500];
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = new ProjectileVertex() {
                    Active = false,
                    WhoAmI = i,
                    Type = 0,
                    Position = Vector2.Zero,
                    Velocity = Vector2.Zero,
                    Weight = 0f,
                    TimeLeft = 0,
                    ConnectedVertices = new bool[500]
                };
            }
        }
        /// <summary>
        /// Creates a new vertex. Returns -1 if it could not create a vertex
        /// </summary>
        public int NewVertex(int type, Vector2 position, Vector2 velocity, float weight, int timeLeft, int connectedTo = -1) {
            for (int i = 0; i < Vertices.Length; i++)
            {
                if (!Vertices[i].Active)
                {
                    Vertices[i] = new ProjectileVertex {
                        Active = true,
                        Velocity = velocity,
                        Type = type,
                        WhoAmI = i,
                        Position = position,
                        Weight = weight,
                        TimeLeft = timeLeft,
                        ConnectedVertices = new bool[500]
                    };
                    return i;
                }
            }
            return -1;
        }
        public override void PreUpdateProjectiles() {
            for (int i = 0; i < Vertices.Length; i++)
            {
                ProjectileVertex thisVertex = Vertices[i];
                if (thisVertex.Active)
                {
                    // Determine if each vertex should be connected
                    for (int j = 0; j < Vertices.Length; j++)
                    {
                        // Try to add a connection to this vertex
                        ProjectileVertex potentialVertex = Vertices[j];
                        if (potentialVertex.Active && potentialVertex.Type == thisVertex.Type && i != j && thisVertex.Weight * potentialVertex.Weight > thisVertex.Position.DistanceSQ(potentialVertex.Position))
                        {
                            thisVertex.ConnectedVertices[j] = true;
                            potentialVertex.ConnectedVertices[i] = true;
                        }
                        // logic for connected vertices
                        if (thisVertex.ConnectedVertices[j])
                        {
                            // distance corrections
                            float distance = (thisVertex.Position + thisVertex.Velocity).DistanceSQ(potentialVertex.Position + potentialVertex.Velocity);
                            // Potentially break the vertices if they are _too_ far
                            if (distance >= 1.2f * thisVertex.Weight * 1.2f * potentialVertex.Weight)
                            {
                                thisVertex.ConnectedVertices[j] = false;
                                potentialVertex.ConnectedVertices[i] = false;
                            }
                            // medium range, pull vertices together
                            else if (distance >= thisVertex.Weight * potentialVertex.Weight)
                            {
                                float strength = distance / (thisVertex.Weight * potentialVertex.Weight);
                                thisVertex.Velocity += strength * 0.03f * thisVertex.Position.DirectionTo(potentialVertex.Position);
                                potentialVertex.Velocity -= strength * 0.03f * thisVertex.Position.DirectionTo(potentialVertex.Position);
                            }
                        }
                    }
                    // Check for terrain
                    Vector2 newVelocity = Collision.TileCollision(thisVertex.Position, thisVertex.Velocity, 2, 2, fallThrough: true, fall2: true);
                    if (newVelocity != thisVertex.Velocity)
                    {
                        // ""reflection""
                        if (newVelocity.X != thisVertex.Velocity.X)
                        {
                            thisVertex.Velocity.X = -0.25f * newVelocity.X;
                        }
                        if (newVelocity.Y != thisVertex.Velocity.Y)
                        {
                            thisVertex.Velocity.Y = -0.25f * newVelocity.Y;
                        }
                    }

                    // apply velocity
                    thisVertex.Position += thisVertex.Velocity;

                    // cool enemies
                    foreach (var npc in Main.ActiveNPCs)
                    {
                        if (npc.Hitbox.Contains((int)thisVertex.Position.X, (int)thisVertex.Position.Y))
                        {
                            var newNPC = npc.realLife != -1 && Main.npc[npc.realLife].active ? Main.npc[npc.realLife] : npc;
                            newNPC.ChangeTemperature(-8);
                        }
                    }
                    // kill
                    if (thisVertex.TimeLeft-- <= 0)
                    {
                        thisVertex.Active = false;
                        Vertices[i] = new ProjectileVertex {
                            Type = 0,
                            Active = false,
                            WhoAmI = i,
                            Position = default,
                            Velocity = default,
                            Weight = 0f,
                            TimeLeft = 0,
                            ConnectedVertices = new bool[500]
                        };
                    }
                }
            }
        }
        public override void PostDrawTiles() {
            Main.spriteBatch.BeginWithDefaults();
            for (int i = 0; i < Vertices.Length; i++)
            {
                if (Vertices[i].Active)
                {
                    ProjectileVertex thisVertex = Vertices[i];
                    /*for (int j = i; j < Vertices.Length; j++)
                    {
                        if (thisVertex.ConnectedVertices[j])
                        {
                            // draw connections (edges)
                            Vector2 center = new((int)((thisVertex.Position.X + Vertices[j].Position.X) / 2), (int)((thisVertex.Position.Y + Vertices[j].Position.Y) / 2));
                            float rotation = thisVertex.Position.DirectionTo(Vertices[j].Position).ToRotation();
                            int width = (int)thisVertex.Position.Distance(Vertices[j].Position);
                            Main.EntitySpriteDraw(new DrawData(Assets.WhitePixel.Value, new Rectangle((int)center.X - (int)Main.screenPosition.X, (int)center.Y - (int)Main.screenPosition.Y, width / 2, 2), null, Color.White, rotation, new Vector2(), SpriteEffects.None));
                        }
                    }*/
                    // draw vertex
                    Main.EntitySpriteDraw(new DrawData(Assets.WhitePixel.Value, new Rectangle((int)thisVertex.Position.X - (int)Main.screenPosition.X, (int)thisVertex.Position.Y - (int)Main.screenPosition.Y, 4, 4), Color.Aqua));
                }
            }
            Main.spriteBatch.End();
        }
    }
}
