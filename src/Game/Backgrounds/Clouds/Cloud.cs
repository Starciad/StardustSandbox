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

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Interfaces.Collections;
using StardustSandbox.Randomness;

using System;

namespace StardustSandbox.Backgrounds.Clouds
{
    internal sealed class Cloud : IPoolableObject
    {
        internal Rectangle SourceRectangle => this.textureRectangle;
        internal Vector2 Position => this.position;

        private Rectangle textureRectangle;
        private Vector2 position;
        private float speed;
        private float opacity;
        private Color color;

        public void Reset()
        {
            this.position = new(-(WorldConstants.GRID_SIZE * 5), SSRandom.Range(0, WorldConstants.GRID_SIZE * 10));
            this.speed = SSRandom.Range(10, 50);
            this.opacity = (SSRandom.GetFloat() * 0.5f) + 0.5f;

            this.color = Color.White;
            this.color.A = Convert.ToByte(255 * this.opacity);
        }

        internal Cloud()
        {
            Reset();
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
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.BgoClouds), this.position, this.textureRectangle, this.color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        internal void SetTextureRectangle(Rectangle rectangle)
        {
            this.textureRectangle = rectangle;
        }
    }
}

