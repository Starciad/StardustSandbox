using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Actors;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.Managers;
using StardustSandbox.Mathematics;
using StardustSandbox.Randomness;
using StardustSandbox.Serialization.Saving.Data;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Actors.Common
{
    internal sealed class GulActor : Actor
    {
        private enum FaceDirection : sbyte
        {
            Left = -1,
            Right = 1,
        }

        private enum State : byte
        {
            Idle,
            Walking
        }

        private FaceDirection direction;
        private State state;
        private ElementIndex storedElement;

        private bool isGrounded;

        private static readonly HashSet<ElementIndex> pickupableElements =
        [
            ElementIndex.Dirt,
            ElementIndex.Mud,
            ElementIndex.Stone,
            ElementIndex.Grass,
            ElementIndex.Ice,
            ElementIndex.Sand,
            ElementIndex.Snow,
            ElementIndex.MovableCorruption,
            ElementIndex.Glass,
            ElementIndex.Iron,
            ElementIndex.Wood,
            ElementIndex.ImmovableCorruption,
            ElementIndex.RedBrick,
            ElementIndex.TreeLeaf,
            ElementIndex.MountingBlock,
            ElementIndex.Lamp,
            ElementIndex.Salt,
            ElementIndex.Bomb,
            ElementIndex.Dynamite,
            ElementIndex.Tnt,
            ElementIndex.DrySponge,
            ElementIndex.WetSponge,
            ElementIndex.Gold,
            ElementIndex.Ash,
            ElementIndex.AntiCorruption,
            ElementIndex.DryBlackWool,
            ElementIndex.DryWhiteWool,
            ElementIndex.DryRedWool,
            ElementIndex.DryOrangeWool,
            ElementIndex.DryYellowWool,
            ElementIndex.DryGreenWool,
            ElementIndex.DryGrayWool,
            ElementIndex.DryBlueWool,
            ElementIndex.DryVioletWool,
            ElementIndex.DryBrownWool,
            ElementIndex.WetBlackWool,
            ElementIndex.WetWhiteWool,
            ElementIndex.WetRedWool,
            ElementIndex.WetOrangeWool,
            ElementIndex.WetYellowWool,
            ElementIndex.WetGreenWool,
            ElementIndex.WetGrayWool,
            ElementIndex.WetBlueWool,
            ElementIndex.WetVioletWool,
            ElementIndex.WetBrownWool,
            ElementIndex.FertileSoil,
            ElementIndex.Seed,
            ElementIndex.Sapling,
            ElementIndex.Moss,
            ElementIndex.Gunpowder,
            ElementIndex.Obsidian,
        ];

        internal GulActor(ActorIndex index, ActorManager actorManager, World world) : base(index, actorManager, world)
        {
            Reset();
        }

        public override void Reset()
        {
            this.direction = SSRandom.GetBool() ? FaceDirection.Left : FaceDirection.Right;
            this.state = State.Idle;
            this.isGrounded = false;
            this.storedElement = ElementIndex.None;
        }

        internal override void Update(GameTime gameTime)
        {
            // If spawned inside a non-empty slot, destroy immediately
            if (!this.world.IsEmptySlotLayer(this.Position, Layer.Foreground))
            {
                this.actorManager.Destroy(this);
                return;
            }

            // Check grounded (world y grows downward)
            this.isGrounded = !this.world.IsEmptySlotLayer(new(this.Position.X, this.Position.Y + 1), Layer.Foreground);

            // Apply gravity if not grounded
            if (!this.isGrounded)
            {
                this.Position = new(this.Position.X, this.Position.Y + 1);
                return;
            }

            // Try to use inventory to build
            if (this.storedElement != ElementIndex.None && SSRandom.Chance(35))
            {
                PlaceStoredElement();
                return;
            }

            // State machine
            UpdateState();
        }

        private void UpdateState()
        {
            if (!this.isGrounded)
            {
                return;
            }

            switch (this.state)
            {
                case State.Idle:
                    UpdateIdleState();
                    break;

                case State.Walking:
                    UpdateWalkState();
                    break;

                default:
                    break;
            }
        }

        private void UpdateIdleState()
        {
            if (SSRandom.Chance(10, 350))
            {
                this.state = State.Walking;
                return;
            }

            if (SSRandom.Chance(5, 100))
            {
                this.direction = (FaceDirection)(-(sbyte)this.direction);
            }
        }

        private void UpdateWalkState()
        {
            if (SSRandom.Chance(10, 350))
            {
                this.state = State.Idle;
                return;
            }

            if (SSRandom.Chance(10, 100))
            {
                Point forward = new(this.Position.X + (sbyte)this.direction, this.Position.Y);
                Point below = new(this.Position.X, this.Position.Y + 1);
                Point above = new(this.Position.X, this.Position.Y - 1);

                // If forward is empty, simply move
                if (this.world.IsEmptySlotLayer(forward, Layer.Foreground))
                {
                    this.Position = forward;
                    return;
                }

                if (SSRandom.GetBool())
                {
                    // Try to collect element in front, above, or below
                    switch (SSRandom.Range(0, 2))
                    {
                        case 0:
                            CollectAt(forward);
                            break;

                        case 1:
                            CollectAt(above);
                            break;

                        case 2:
                            CollectAt(below);
                            break;

                        default:
                            CollectAt(forward);
                            break;
                    }
                }
                else
                {
                    // Attempt to climb: if forward is occupied but the element above forward is empty, move on top of it
                    Point aboveForward = new(forward.X, forward.Y - 1);

                    if (this.world.IsWithinBounds(aboveForward) && this.world.IsEmptySlotLayer(aboveForward, Layer.Foreground))
                    {
                        // Ensure we have space above our current position as well (not strictly necessary for 1x1, but safer)
                        Point aboveCurrent = new(this.Position.X, this.Position.Y - 1);

                        if (this.world.IsWithinBounds(aboveCurrent) && this.world.IsEmptySlotLayer(aboveCurrent, Layer.Foreground))
                        {
                            // Move to the top of the forward block
                            this.Position = aboveForward;
                        }
                    }
                }
            }
        }

        private void CollectAt(Point slot)
        {
            // If no element, nothing to collect
            if (!this.world.TryGetElement(slot, Layer.Foreground, out ElementIndex elementIndex))
            {
                return;
            }

            // Determine if element is collectible
            if (!pickupableElements.Contains(elementIndex))
            {
                return;
            }

            // Remove the element from the world and store it
            this.world.RemoveElement(slot, Layer.Foreground);
            this.storedElement = elementIndex;
        }

        private void PlaceStoredElement()
        {
            Point forward = new(this.Position.X + (sbyte)this.direction, this.Position.Y);

            if (this.storedElement is ElementIndex.None || this.actorManager.HasEntityAtPosition(forward))
            {
                return;
            }

            if (this.world.IsEmptySlotLayer(forward, Layer.Foreground))
            {
                this.world.InstantiateElement(forward, Layer.Foreground, this.storedElement);
                this.storedElement = ElementIndex.None;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                AssetDatabase.GetTexture(TextureIndex.ActorGul),
                new Rectangle(this.Position.X * SpriteConstants.SPRITE_SCALE,
                              this.Position.Y * SpriteConstants.SPRITE_SCALE,
                              SpriteConstants.SPRITE_SCALE,
                              SpriteConstants.SPRITE_SCALE),
                new Rectangle(0, this.direction == FaceDirection.Right ? 0 : 32, 32, 32),
                Color.White,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f
            );
        }

        internal override ActorData Serialize()
        {
            return new()
            {
                Index = this.Index,
                Content = new Dictionary<string, object>()
                {
                    ["PositionX"] = this.Position.X,
                    ["PositionY"] = this.Position.Y,
                },
            };
        }

        internal override void Deserialize(ActorData data)
        {
            if (data.Content.TryGetValue("PositionX", out object positionXObj) && positionXObj is int positionX)
            {
                this.Position = new(positionX, this.Position.Y);
            }

            if (data.Content.TryGetValue("PositionY", out object positionYObj) && positionYObj is int positionY)
            {
                this.Position = new(this.Position.X, positionY);
            }
        }
    }
}
