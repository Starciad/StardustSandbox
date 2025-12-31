using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Actors;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.Managers;
using StardustSandbox.Randomness;
using StardustSandbox.Serialization.Saving.Data;
using StardustSandbox.WorldSystem;

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

        private FaceDirection direction;
        private ElementIndex grabbedElementIndex;

        private Point collectedElementPosition;
        private Point placedElementPosition;

        private Element GrabbedElement => ElementDatabase.GetElement(this.grabbedElementIndex);
        private bool IsGrabbingElement => this.grabbedElementIndex is not ElementIndex.None;
        private bool IsFalling => !this.IsGrounded;
        private bool IsGrounded => HasGroundBelow(this.Position);

        private static readonly HashSet<ElementIndex> grabbableElements =
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

        private static readonly HashSet<Point> possiblePositions = [];

        internal GulActor(ActorIndex index, ActorManager actorManager, World world) : base(index, actorManager, world)
        {
            Reset();
        }

        public override void Reset()
        {
            this.direction = SSRandom.GetBool() ? FaceDirection.Left : FaceDirection.Right;
            this.grabbedElementIndex = ElementIndex.None;

            this.collectedElementPosition = Point.Zero;
            this.placedElementPosition = Point.Zero;
        }

        internal override void Update(GameTime gameTime)
        {
            // If spawned inside a non-empty slot, destroy immediately
            if (!this.world.IsEmptySlotLayer(this.Position, Layer.Foreground))
            {
                this.actorManager.Destroy(this);
                return;
            }

            if (this.IsFalling)
            {
                // Apply gravity
                this.Position = new(this.Position.X, this.Position.Y + 1);
            }
            else
            {
                // Perform behavior only when grounded
                UpdateBehavior();
            }
        }

        private void UpdateBehavior()
        {
            if (this.IsGrabbingElement)
            {
                UpdateGrabbedElementBehavior();
            }
            else
            {
                UpdateNormalBehavior();
            }
        }

        private void UpdateGrabbedElementBehavior()
        {
            /*
             * This behavior includes:
             * 
             * 1. Move away from the location where you collected the element.
             * 2. Try to position the element in another location, avoiding placing it on top of other entities.
             */
        }

        private void UpdateNormalBehavior()
        {
            /*
             * This behavior includes:
             *
             * 1. Observing the surroundings
             * 2. Walking in any direction
             * 3. Collecting an item
             */

            // Idle
            if (SSRandom.Chance(5))
            {
                TurnAround();
                return;
            }

            // Walking
            if (SSRandom.Chance(10))
            {
                possiblePositions.Clear();
                _ = possiblePositions.Add(new(this.Position.X + (sbyte)this.direction, this.Position.Y - 1));
                _ = possiblePositions.Add(new(this.Position.X + (sbyte)this.direction, this.Position.Y));
                _ = possiblePositions.Add(new(this.Position.X + (sbyte)this.direction, this.Position.Y + 1));
                _ = possiblePositions.RemoveWhere(point => !this.world.IsEmptySlotLayer(point, Layer.Foreground) || !HasGroundBelow(point));

                if (possiblePositions.Count > 0)
                {
                    this.Position = possiblePositions.GetRandomItem();
                }
                else
                {
                    TurnAround();
                }
            }
        }

        private void TurnAround()
        {
            this.direction = this.direction == FaceDirection.Left ? FaceDirection.Right : FaceDirection.Left;
        }

        private bool HasGroundBelow(Point position)
        {
            return !this.world.IsEmptySlotLayer(new(position.X, position.Y + 1), Layer.Foreground);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                AssetDatabase.GetTexture(TextureIndex.ActorGul),
                new Rectangle(this.Position.X * SpriteConstants.SPRITE_SCALE, this.Position.Y * SpriteConstants.SPRITE_SCALE, SpriteConstants.SPRITE_SCALE, SpriteConstants.SPRITE_SCALE),
                new Rectangle(0, this.direction == FaceDirection.Right ? 0 : 32, 32, 32),
                Color.White,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f
            );

            if (this.IsGrabbingElement)
            {
                spriteBatch.Draw(
                    AssetDatabase.GetTexture(TextureIndex.Elements),
                    new(
                        (this.Position.X * SpriteConstants.SPRITE_SCALE) + (this.direction is FaceDirection.Right ? 12.0f * (float)this.direction : 4.0f),
                        (this.Position.Y * SpriteConstants.SPRITE_SCALE) + 16.0f
                    ),
                    new(this.GrabbedElement.RenderingType switch
                    {
                        ElementRenderingType.Single => Point.Zero,
                        ElementRenderingType.Blob => new(32, 0),
                        _ => Point.Zero,
                    } + this.GrabbedElement.TextureOriginOffset, new(SpriteConstants.SPRITE_SCALE)),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    new Vector2(0.5f),
                    SpriteEffects.None,
                    0.0f
                );
            }
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
