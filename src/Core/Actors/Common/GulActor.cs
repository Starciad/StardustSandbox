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

        private Direction direction;
        private ElementIndex grabbedElementIndex;
        private Point positionElementPlaced;
        private uint elementsPlacedCount;

        private static readonly List<Point> possiblePositions = [];

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

        private static bool IsGrabbableElement(ElementIndex elementIndex)
        {
            return elementIndex switch
            {
                ElementIndex.Dirt or
                ElementIndex.Mud or
                ElementIndex.Stone or
                ElementIndex.Grass or
                ElementIndex.Ice or
                ElementIndex.Sand or
                ElementIndex.Snow or
                ElementIndex.Glass or
                ElementIndex.Iron or
                ElementIndex.Wood or
                ElementIndex.RedBrick or
                ElementIndex.TreeLeaf or
                ElementIndex.MountingBlock or
                ElementIndex.LampOn or
                ElementIndex.Salt or
                ElementIndex.Bomb or
                ElementIndex.Dynamite or
                ElementIndex.Tnt or
                ElementIndex.DrySponge or
                ElementIndex.WetSponge or
                ElementIndex.Gold or
                ElementIndex.Ash or
                ElementIndex.AntiCorruption or
                ElementIndex.DryBlackWool or
                ElementIndex.DryWhiteWool or
                ElementIndex.DryRedWool or
                ElementIndex.DryOrangeWool or
                ElementIndex.DryYellowWool or
                ElementIndex.DryGreenWool or
                ElementIndex.DryGrayWool or
                ElementIndex.DryBlueWool or
                ElementIndex.DryVioletWool or
                ElementIndex.DryBrownWool or
                ElementIndex.WetBlackWool or
                ElementIndex.WetWhiteWool or
                ElementIndex.WetRedWool or
                ElementIndex.WetOrangeWool or
                ElementIndex.WetYellowWool or
                ElementIndex.WetGreenWool or
                ElementIndex.WetGrayWool or
                ElementIndex.WetBlueWool or
                ElementIndex.WetVioletWool or
                ElementIndex.WetBrownWool or
                ElementIndex.FertileSoil or
                ElementIndex.Seed or
                ElementIndex.Sapling or
                ElementIndex.Moss or
                ElementIndex.Gunpowder or
                ElementIndex.Obsidian => true,
                _ => false,
            };
        }

        private static bool IsMortalElement(ElementIndex elementIndex)
        {
            return elementIndex switch
            {
                ElementIndex.Water or
                ElementIndex.MovableCorruption or
                ElementIndex.Lava or
                ElementIndex.Acid or
                ElementIndex.GasCorruption or
                ElementIndex.LiquidCorruption or
                ElementIndex.ImmovableCorruption or
                ElementIndex.Smoke or
                ElementIndex.Fire or
                ElementIndex.Void or
                ElementIndex.Oil or
                ElementIndex.Salt or
                ElementIndex.Saltwater or
                ElementIndex.Devourer or
                ElementIndex.LiquefiedPetroleumGas or
                ElementIndex.BlackPaint or
                ElementIndex.WhitePaint or
                ElementIndex.RedPaint or
                ElementIndex.OrangePaint or
                ElementIndex.YellowPaint or
                ElementIndex.GreenPaint or
                ElementIndex.BluePaint or
                ElementIndex.GrayPaint or
                ElementIndex.VioletPaint or
                ElementIndex.BrownPaint or
                ElementIndex.Mercury or
                ElementIndex.Electricity => true,
                _ => false,
            };
        }

        private bool IsBeingSuffocated(Point position)
        {
            return !this.world.IsEmptySlotLayer(position, Layer.Foreground);
        }

        private bool IsOnTopMortalElement(Point position)
        {
            return this.world.TryGetSlotLayer(new(position.X, position.Y + 1), Layer.Foreground, out SlotLayer slotLayer) && !slotLayer.IsEmpty &&
                   (IsMortalElement(slotLayer.ElementIndex) || slotLayer.Temperature < -15.0f || slotLayer.Temperature > 48.0f);
        }

        private void TurnAround()
        {
            this.direction = this.direction == Direction.Left ? Direction.Right : Direction.Left;
        }

        private bool HasEntityAbove(Point position)
        {
            return this.actorManager.HasEntityAtPosition(new(position.X, position.Y - 1));
        }

        private void SetFrontPositions(Predicate<Point> removeMatch)
        {
            possiblePositions.Clear();
            possiblePositions.Add(new(this.PositionX + (sbyte)this.direction, this.PositionY - 1));
            possiblePositions.Add(new(this.PositionX + (sbyte)this.direction, this.PositionY));
            possiblePositions.Add(new(this.PositionX + (sbyte)this.direction, this.PositionY + 1));
            _ = possiblePositions.RemoveAll(removeMatch);
        }

        private bool CanWalkTo(Point position)
        {
            return this.world.IsEmptySlotLayer(position, Layer.Foreground) &&
                   IsInsideWorldBounds(position) &&
                   IsGrounded(position) &&
                   !IsOnTopMortalElement(position);
        }

        private bool TryWalk()
        {
            SetFrontPositions(point => !this.world.IsEmptySlotLayer(point, Layer.Foreground) || !IsGrounded(point));

            while (possiblePositions.Count > 0)
            {
                Point position = possiblePositions.GetRandomItem();

                if (!CanWalkTo(position))
                {
                    _ = possiblePositions.Remove(position);
                    continue;
                }

                SetPosition(position);
                return true;
            }

            return false;
        }

        private bool TryGrabElement()
        {
            SetFrontPositions(point =>
                this.world.IsEmptySlotLayer(point, Layer.Foreground) ||
                !IsGrabbableElement(this.world.GetElement(point, Layer.Foreground)) ||
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
            if (!IsGrounded())
            {
                // Apply gravity
                MoveBy(0, 1);
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
                new(this.PositionX * SpriteConstants.SPRITE_SCALE, this.PositionY * SpriteConstants.SPRITE_SCALE, SpriteConstants.SPRITE_SCALE, SpriteConstants.SPRITE_SCALE),
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
                        (this.PositionX * SpriteConstants.SPRITE_SCALE) + (this.direction is Direction.Right ? 12.0f * (float)this.direction : 4.0f),
                        (this.PositionY * SpriteConstants.SPRITE_SCALE) + 16.0f
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
                    ["GrabbedElementIndex"] = this.grabbedElementIndex,
                    ["PositionX"] = this.PositionX,
                    ["PositionY"] = this.PositionY,
                    ["PositionElementPlaced.X"] = this.positionElementPlaced.X,
                    ["PositionElementPlaced.Y"] = this.positionElementPlaced.Y,
                },
            };
        }

        internal override void Deserialize(ActorData data)
        {
            Direction tempDirection = Randomness.Random.GetBool() ? Direction.Left : Direction.Right;
            ElementIndex tempGrabbedElementIndex = ElementIndex.None;
            Point tempPosition = Point.Zero;
            Point tempPositionElementPlaced = Point.Zero;

            if (data.Content.TryGetValue("Direction", out object value))
            {
                tempDirection = (Direction)value;
            }

            if (data.Content.TryGetValue("GrabbedElementIndex", out value))
            {
                tempGrabbedElementIndex = (ElementIndex)value;
            }

            if (data.Content.TryGetValue("PositionX", out value))
            {
                tempPosition.X = (int)value;
            }

            if (data.Content.TryGetValue("PositionY", out value))
            {
                tempPosition.Y = (int)value;
            }

            if (data.Content.TryGetValue("PositionElementPlaced.X", out value))
            {
                tempPositionElementPlaced.X = (int)value;
            }

            if (data.Content.TryGetValue("PositionElementPlaced.Y", out value))
            {
                tempPositionElementPlaced.Y = (int)value;
            }

            this.direction = tempDirection;
            this.grabbedElementIndex = tempGrabbedElementIndex;
            SetPosition(tempPosition);
            this.positionElementPlaced = tempPositionElementPlaced;
        }
    }
}
