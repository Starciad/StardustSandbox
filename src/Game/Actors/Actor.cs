/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

using StardustSandbox.Enums.Actors;
using StardustSandbox.Interfaces.Collections;
using StardustSandbox.Managers;
using StardustSandbox.Serialization.Saving.Data;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Actors
{
    internal abstract class Actor : IPoolableObject
    {
        internal ActorIndex Index { get; }

        internal Point Position { get; set; }
        internal Point Size { get; set; }

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

