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

        internal Rectangle SelfRectangle => new(this.PositionX, this.PositionY, this.Width, this.Height);

        internal Point Position
        {
            get
            {
                return new(this.PositionX, this.PositionY);
            }

            set
            {
                this.PositionX = value.X;
                this.PositionY = value.Y;
            }
        }

        internal Point Size
        {
            get
            {
                return new(this.Width, this.Height);
            }

            set
            {
                this.Width = value.X;
                this.Height = value.Y;
            }
        }

        internal int PositionX { get; set; }
        internal int PositionY { get; set; }
        internal int Width { get; set; }
        internal int Height { get; set; }

        internal bool CanUpdate { get; set; }
        internal bool CanDraw { get; set; }
        internal ActorState State { get; set; }

        protected readonly ActorManager actorManager;
        protected readonly World world;

        internal Actor(ActorIndex index, ActorManager actorManager, World world)
        {
            this.Index = index;
            this.actorManager = actorManager;
            this.world = world;
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
