/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Achievements;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Enums.Actors;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization.Saving.Data;
using StardustSandbox.Core.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Actors.Common
{
    internal sealed class GulActor : Actor
    {
        private enum Direction : sbyte
        {
            Left = -1,
            Right = 1,
        }

        private Element GrabbedElement => ElementDatabase.GetElement(this.grabbedElementIndex);
        private bool IsGrabbingElement => this.grabbedElementIndex is not ElementIndex.None;
        private bool IsFalling => !this.IsGrounded;
        private bool IsGrounded => HasGroundBelow(this.Position);

        private Direction direction;
        private ElementIndex grabbedElementIndex;
        private Point positionElementPlaced;
        private uint elementsPlacedCount;

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
            ElementIndex.LampOn,
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
            this.direction = Randomness.Random.GetBool() ? Direction.Left : Direction.Right;
            this.grabbedElementIndex = ElementIndex.None;
            this.positionElementPlaced = new(-1);
        }

        private bool IsBeingSuffocated(Point position)
        {
            return !this.world.IsEmptySlotLayer(position, Layer.Foreground);
        }

        private bool IsOnTopMortalElement(Point position)
        {
            return this.world.TryGetSlotLayer(new(position.X, position.Y + 1), Layer.Foreground, out SlotLayer slotLayer) && !slotLayer.IsEmpty &&
                   (mortalElements.Contains(slotLayer.ElementIndex) || slotLayer.Temperature < -15.0f || slotLayer.Temperature > 48.0f);
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

        private void SetFrontPositions(Predicate<Point> removeMatch)
        {
            possiblePositions.Clear();
            _ = possiblePositions.Add(new(this.Position.X + (sbyte)this.direction, this.Position.Y - 1));
            _ = possiblePositions.Add(new(this.Position.X + (sbyte)this.direction, this.Position.Y));
            _ = possiblePositions.Add(new(this.Position.X + (sbyte)this.direction, this.Position.Y + 1));
            _ = possiblePositions.RemoveWhere(removeMatch);
        }

        private bool CanWalkTo(Point position)
        {
            return this.world.IsEmptySlotLayer(position, Layer.Foreground) &&
                   HasGroundBelow(position) &&
                   !IsOnTopMortalElement(position);
        }

        private bool TryWalk()
        {
            SetFrontPositions(point => !this.world.IsEmptySlotLayer(point, Layer.Foreground) || !HasGroundBelow(point));

            while (possiblePositions.Count > 0)
            {
                Point position = possiblePositions.GetRandomItem();

                if (!CanWalkTo(position))
                {
                    _ = possiblePositions.Remove(position);
                    continue;
                }

                this.Position = position;
                return true;
            }

            return false;
        }

        private bool TryGrabElement()
        {
            SetFrontPositions(point =>
                this.world.IsEmptySlotLayer(point, Layer.Foreground) ||
                !grabbableElements.Contains(this.world.GetElement(point, Layer.Foreground)) ||
                HasEntityAbove(point)
            );

            while (possiblePositions.Count > 0)
            {
                Point position = possiblePositions.GetRandomItem();

                if (this.world.TryGetElement(position, Layer.Foreground, out ElementIndex index) && IsOnTopMortalElement(new(position.X, position.Y - 1)))
                {
                    _ = possiblePositions.Remove(position);
                    continue;
                }

                this.grabbedElementIndex = index;
                this.world.RemoveElement(position, Layer.Foreground);

                return true;
            }

            return false;
        }

        private void IncrementElementsPlacedCount()
        {
            this.elementsPlacedCount =
                this.elementsPlacedCount == uint.MaxValue
                    ? uint.MaxValue
                    : this.elementsPlacedCount + 1;

            if (this.elementsPlacedCount >= 100)
            {
                AchievementEngine.Unlock(AchievementIndex.ACH_007);
            }
        }

        private bool TryPlaceElement()
        {
            SetFrontPositions(point =>
                !this.world.IsEmptySlotLayer(point, Layer.Foreground) ||
                this.actorManager.HasEntityAtPosition(point) ||
                point == this.positionElementPlaced
            );

            while (possiblePositions.Count > 0)
            {
                Point position = possiblePositions.GetRandomItem();

                if (!this.world.TryInstantiateElement(position, Layer.Foreground, this.grabbedElementIndex))
                {
                    _ = possiblePositions.Remove(position);
                    continue;
                }

                this.grabbedElementIndex = ElementIndex.None;
                this.positionElementPlaced = position;

                IncrementElementsPlacedCount();

                return true;
            }

            return false;
        }

        internal override void Update(GameTime gameTime)
        {
            // If spawned inside a non-empty slot, destroy immediately
            if (IsBeingSuffocated(this.Position) || IsOnTopMortalElement(this.Position))
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
            // Try to walk or place element
            if (Randomness.Random.Chance(40) && !TryWalk() && !TryPlaceElement())
            {
                // Turn around if unable to walk or place element
                TurnAround();
            }
        }

        private void UpdateNormalBehavior()
        {
            // Try to walk or grab element
            if (Randomness.Random.Chance(40) && !TryWalk() && !TryGrabElement())
            {
                // Turn around if unable to walk or grab element
                TurnAround();
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                AssetDatabase.GetTexture(TextureIndex.Actors),
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
                    ["Direction"] = this.direction,
                    ["Position.X"] = this.Position.X,
                    ["Position.Y"] = this.Position.Y,
                    ["GrabbedElementIndex"] = this.grabbedElementIndex,
                    ["PositionElementPlaced"] = this.positionElementPlaced,
                },
            };
        }

        internal override void Deserialize(ActorData data)
        {
            if (data.Content.TryGetValue("Direction", out object directionObj) && directionObj is Direction direction)
            {
                this.direction = direction;
            }

            if (data.Content.TryGetValue("Position.X", out object positionXObj) && positionXObj is int positionX)
            {
                this.Position = new(positionX, this.Position.Y);
            }

            if (data.Content.TryGetValue("Position.Y", out object positionYObj) && positionYObj is int positionY)
            {
                this.Position = new(this.Position.X, positionY);
            }

            if (data.Content.TryGetValue("GrabbedElementIndex", out object grabbedElementIndexObj) && grabbedElementIndexObj is ElementIndex grabbedElementIndex)
            {
                this.grabbedElementIndex = grabbedElementIndex;
            }

            if (data.Content.TryGetValue("PositionElementPlaced", out object positionElementPlacedObj) && positionElementPlacedObj is Point positionElementPlaced)
            {
                this.positionElementPlaced = positionElementPlaced;
            }
        }
    }
}

