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

using StardustSandbox.Core.Enums.Actors;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization.Saving.Data;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core.Actors
{
    internal abstract class Actor : IPoolableObject
    {
        internal ActorIndex Index { get; }

        internal Rectangle SelfRectangle => new(this.positionX, this.positionY, this.width, this.height);

        internal Point Position
        {
            get => new(this.positionX, this.positionY);

            private set
            {
                this.positionX = value.X;
                this.positionY = value.Y;
            }
        }

        internal Point Size
        {
            get => new(this.width, this.height);

            set
            {
                this.width = value.X;
                this.height = value.Y;
            }
        }

        internal int PositionX => this.positionX;
        internal int PositionY => this.positionY;
        internal int Width => this.width;
        internal int Height => this.height;

        internal bool CanUpdate { get; set; }
        internal bool CanDraw { get; set; }
        internal ActorState State { get; set; }

        private int positionX;
        private int positionY;
        private int width;
        private int height;

        protected readonly ActorManager actorManager;
        protected readonly World world;

        internal Actor(ActorIndex index, ActorManager actorManager, World world)
        {
            this.Index = index;
            this.actorManager = actorManager;
            this.world = world;
        }

        internal void ClampToWorld()
        {
            Point clamped = ClampPositionToWorld(this.Position, this.Size);
            this.positionX = clamped.X;
            this.positionY = clamped.Y;
        }

        private Point ClampPositionToWorld(Point position, Point size)
        {
            int maxX = Math.Max(0, this.world.Information.Size.X - size.X);
            int maxY = Math.Max(0, this.world.Information.Size.Y - size.Y);

            if (position.X < 0)
            {
                position.X = 0;
            }
            else if (position.X > maxX)
            {
                position.X = maxX;
            }

            if (position.Y < 0)
            {
                position.Y = 0;
            }
            else if (position.Y > maxY)
            {
                position.Y = maxY;
            }

            return new(position.X, position.Y);
        }

        internal bool IsInsideWorldBounds(Rectangle rectangle)
        {
            return rectangle.Left >= 0
                && rectangle.Top >= 0
                && rectangle.Right <= this.world.Information.Size.X
                && rectangle.Bottom <= this.world.Information.Size.Y;
        }

        internal bool IsInsideWorldBounds(Point position, Point size)
        {
            return IsInsideWorldBounds(new Rectangle(position.X, position.Y, size.X, size.Y));
        }

        internal bool IsInsideWorldBounds(Point position)
        {
            return IsInsideWorldBounds(new Rectangle(position.X, position.Y, this.Size.X, this.Size.Y));
        }

        internal void SetPosition(Point newPos, bool shouldClamp = true)
        {
            if (shouldClamp)
            {
                Point clamped = ClampPositionToWorld(newPos, this.Size);

                this.positionX = clamped.X;
                this.positionY = clamped.Y;
            }
            else
            {
                this.positionX = newPos.X;
                this.positionY = newPos.Y;
            }
        }

        internal bool TrySetPosition(Point newPosition)
        {
            Rectangle rect = new(newPosition, this.Size);

            if (IsInsideWorldBounds(rect))
            {
                this.positionX = newPosition.X;
                this.positionY = newPosition.Y;
                return true;
            }

            return false;
        }

        private void MoveAxis(int motion, bool isHorizontalAxis)
        {
            if (motion == 0)
            {
                return;
            }

            int sign = motion > 0 ? 1 : -1;
            int pos = isHorizontalAxis ? this.positionX : this.positionY;
            int otherPos = isHorizontalAxis ? this.positionY : this.positionX;

            Rectangle nextRect = this.SelfRectangle;

            while (motion != 0)
            {
                if (isHorizontalAxis)
                {
                    nextRect.X = pos + sign;
                    nextRect.Y = otherPos;
                }
                else
                {
                    nextRect.Y = pos + sign;
                    nextRect.X = otherPos;
                }

                if (!IsInsideWorldBounds(nextRect))
                {
                    break;
                }

                pos += sign;
                motion -= sign;
            }

            if (isHorizontalAxis)
            {
                this.positionX = pos;
            }
            else
            {
                this.positionY = pos;
            }
        }

        internal void MoveBy(int deltaX, int deltaY)
        {
            if (deltaX != 0)
            {
                MoveAxis(deltaX, true);
            }

            if (deltaY != 0)
            {
                MoveAxis(deltaY, false);
            }
        }

        internal bool IsGrounded(Point position, Point size)
        {
            int startX = position.X;
            int endX = startX + size.X;
            int belowY = position.Y + size.Y;
            int x = startX;

            Point belowPosition = new(startX, belowY);

            while (x < endX)
            {
                belowPosition.X = x;

                // If you move the entity below the boundaries, consider it "grounded"
                if (!IsInsideWorldBounds(new Rectangle(belowPosition, size)))
                {
                    return true;
                }

                // Only query the layer if it is within the limits.
                if (this.world.TryGetSlotLayer(belowPosition, Layer.Foreground, out SlotLayer slotLayer))
                {
                    ElementCategory category = slotLayer.Element.Category;

                    if (category == ElementCategory.MovableSolid || category == ElementCategory.ImmovableSolid)
                    {
                        return true;
                    }
                }

                x++;
            }

            return false;
        }

        internal bool IsGrounded(Point position)
        {
            return IsGrounded(position, this.Size);
        }

        internal bool IsGrounded()
        {
            return IsGrounded(this.Position, this.Size);
        }

        public abstract void Reset();
        internal abstract void Update(GameTime gameTime);
        internal abstract void Draw(SpriteBatch spriteBatch);

        internal virtual void OnCreated() { return; }
        internal virtual void OnDestroyed() { return; }

        internal abstract ActorData Serialize();
        internal abstract void Deserialize(ActorData data);
    }
}
