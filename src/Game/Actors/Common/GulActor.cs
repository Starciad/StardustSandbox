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

using System;
using System.Collections.Generic;

namespace StardustSandbox.Actors.Common
{
    internal sealed class GulActor : Actor
    {
        private enum Direction : sbyte
        {
            Left = -1,
            Right = 1,
        }

        private Direction direction;
        private ElementIndex grabbedElementIndex;
        private Point positionElementPositioned;

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
            ElementIndex.Glass,
            ElementIndex.Iron,
            ElementIndex.Wood,
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

        private static readonly HashSet<ElementIndex> mortalElements =
        [
            ElementIndex.Water,
            ElementIndex.MovableCorruption,
            ElementIndex.Lava,
            ElementIndex.Acid,
            ElementIndex.GasCorruption,
            ElementIndex.LiquidCorruption,
            ElementIndex.ImmovableCorruption,
            ElementIndex.Smoke,
            ElementIndex.Fire,
            ElementIndex.Void,
            ElementIndex.Clone,
            ElementIndex.Oil,
            ElementIndex.Salt,
            ElementIndex.Saltwater,
            ElementIndex.Devourer,
            ElementIndex.LiquefiedPetroleumGas,
            ElementIndex.BlackPaint,
            ElementIndex.WhitePaint,
            ElementIndex.RedPaint,
            ElementIndex.OrangePaint,
            ElementIndex.YellowPaint,
            ElementIndex.GreenPaint,
            ElementIndex.BluePaint,
            ElementIndex.GrayPaint,
            ElementIndex.VioletPaint,
            ElementIndex.BrownPaint,
            ElementIndex.Mercury,
        ];

        private static readonly HashSet<Point> possiblePositions = [];

        internal GulActor(ActorIndex index, ActorManager actorManager, World world) : base(index, actorManager, world)
        {
            Reset();
        }

        public override void Reset()
        {
            this.direction = SSRandom.GetBool() ? Direction.Left : Direction.Right;
            this.grabbedElementIndex = ElementIndex.None;
            this.positionElementPositioned = new(-1);
        }

        private bool IsBeingSuffocated()
        {
            return !this.world.IsEmptySlotLayer(this.Position, Layer.Foreground);
        }

        private bool IsOnTopMortalElement()
        {
            return this.world.TryGetElement(new(this.Position.X, this.Position.Y + 1), Layer.Foreground, out ElementIndex index) && mortalElements.Contains(index);
        }

        private void TurnAround()
        {
            this.direction = this.direction == Direction.Left ? Direction.Right : Direction.Left;
        }

        private bool HasEntityAbove(Point position)
        {
            return this.actorManager.HasEntityAtPosition(new(position.X, position.Y - 1));
        }

        private bool HasGroundBelow(Point position)
        {
            return this.world.TryGetElement(new(position.X, position.Y + 1), Layer.Foreground, out ElementIndex index) && ElementDatabase.GetElement(index).Category is ElementCategory.MovableSolid or ElementCategory.ImmovableSolid;
        }

        private void SetFrontPositions(bool hasDeepPoint, Predicate<Point> removeMatch)
        {
            possiblePositions.Clear();
            _ = possiblePositions.Add(new(this.Position.X + (sbyte)this.direction, this.Position.Y - 1));
            _ = possiblePositions.Add(new(this.Position.X + (sbyte)this.direction, this.Position.Y));
            _ = possiblePositions.Add(new(this.Position.X + (sbyte)this.direction, this.Position.Y + 1));

            if (hasDeepPoint)
            {
                _ = possiblePositions.Add(new(this.Position.X + (sbyte)this.direction, this.Position.Y + 2));
            }

            _ = possiblePositions.RemoveWhere(removeMatch);
        }

        private bool TryWalk()
        {
            SetFrontPositions(false, point => !this.world.IsEmptySlotLayer(point, Layer.Foreground) || !HasGroundBelow(point));

            if (possiblePositions.Count > 0)
            {
                this.Position = possiblePositions.GetRandomItem();
                return true;
            }

            return false;
        }

        private bool TryGrabElement()
        {
            SetFrontPositions(false, point =>
                this.world.IsEmptySlotLayer(point, Layer.Foreground) ||
                !grabbableElements.Contains(this.world.GetElement(point, Layer.Foreground)) ||
                HasEntityAbove(point)
            );

            if (possiblePositions.Count > 0)
            {
                Point position = possiblePositions.GetRandomItem();

                this.grabbedElementIndex = this.world.GetElement(position, Layer.Foreground);
                this.world.RemoveElement(position, Layer.Foreground);

                return true;
            }

            return false;
        }

        private bool TryPlaceElement()
        {
            SetFrontPositions(true, point =>
                !this.world.IsEmptySlotLayer(point, Layer.Foreground) ||
                this.actorManager.HasEntityAtPosition(point) ||
                point == this.positionElementPositioned
            );

            if (possiblePositions.Count > 0)
            {
                Point position = possiblePositions.GetRandomItem();

                this.world.InstantiateElement(position, Layer.Foreground, this.grabbedElementIndex);
                this.grabbedElementIndex = ElementIndex.None;
                this.positionElementPositioned = position;

                return true;
            }

            return false;
        }

        internal override void Update(GameTime gameTime)
        {
            // If spawned inside a non-empty slot, destroy immediately
            if (IsBeingSuffocated() || IsOnTopMortalElement())
            {
                this.actorManager.Destroy(this);
                return;
            }

            // Falling behavior
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
            // Different behavior based on whether grabbing an element
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
            // 15% chance to act each update
            // Try to walk or place element
            if (SSRandom.Chance(15) && !TryWalk() && !TryPlaceElement())
            {
                // Turn around if unable to walk or place element
                TurnAround();
            }
        }

        private void UpdateNormalBehavior()
        {
            // 15% chance to act each update
            // Try to walk or grab element
            if (SSRandom.Chance(15) && !TryWalk() && !TryGrabElement())
            {
                // Turn around if unable to walk or grab element
                TurnAround();
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                AssetDatabase.GetTexture(TextureIndex.ActorGul),
                new(this.Position.X * SpriteConstants.SPRITE_SCALE, this.Position.Y * SpriteConstants.SPRITE_SCALE, SpriteConstants.SPRITE_SCALE, SpriteConstants.SPRITE_SCALE),
                new(0, this.direction == Direction.Right ? 0 : 32, 32, 32),
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
                        (this.Position.X * SpriteConstants.SPRITE_SCALE) + (this.direction is Direction.Right ? 12.0f * (float)this.direction : 4.0f),
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
                    ["GrabbedElementIndex"] = this.grabbedElementIndex,
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

            if (data.Content.TryGetValue("GrabbedElementIndex", out object grabbedElementIndexObj) && grabbedElementIndexObj is ElementIndex grabbedElementIndex)
            {
                this.grabbedElementIndex = grabbedElementIndex;
            }
        }
    }
}
