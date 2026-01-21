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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.Core.Backgrounds.Clouds
{
    internal sealed class Cloud : IPoolableObject
    {
        internal RectangleF SelfRectangle => new(this.position.X, this.position.Y, this.SourceRectangle.Width, this.SourceRectangle.Height);

        internal Rectangle SourceRectangle { get; set; }
        internal bool IsDestroyed { get; set; }

        private Vector2 position;
        private float speed;
        private float opacity;
        private Color color;

        internal Cloud()
        {
            Reset();
        }

        public void Reset()
        {
            this.position = new(-(WorldConstants.GRID_SIZE * 5), Core.Random.Range(0, WorldConstants.GRID_SIZE * 10));
            this.speed = Core.Random.Range(10, 50);
            this.opacity = (Core.Random.GetFloat() * 0.5f) + 0.5f;
            this.color = new(Color.White, Convert.ToByte(255 * this.opacity));
        }

        internal void Update(GameTime gameTime, SimulationSpeed simulationSpeed)
        {
            float multiplier = simulationSpeed switch
            {
                SimulationSpeed.Normal => 1.0f,
                SimulationSpeed.Fast => 2.0f,
                SimulationSpeed.VeryFast => 4.0f,
                _ => 1.0f,
            };

            this.position.X += Convert.ToSingle(this.speed * multiplier * gameTime.ElapsedGameTime.TotalSeconds);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.BgoClouds), this.position, this.SourceRectangle, this.color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}

