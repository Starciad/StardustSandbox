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

using StardustSandbox.Core;

using System;

namespace StardustSandbox.Core.Backgrounds
{
    internal sealed class BackgroundLayer
    {
        private Vector2 layerPosition = Vector2.Zero;
        private Vector2 movementOffsetPosition = Vector2.Zero;

        private Vector2 horizontalLayerPosition = Vector2.Zero;
        private Vector2 verticalLayerPosition = Vector2.Zero;
        private Vector2 diagonalLayerPosition = Vector2.Zero;

        private readonly Vector2 parallaxFactor;
        private readonly Vector2 movementSpeed;

        private readonly bool lockX;
        private readonly bool lockY;

        internal BackgroundLayer(Vector2 parallaxFactor, Vector2 movementSpeed, bool lockX = false, bool lockY = false)
        {
            this.parallaxFactor = parallaxFactor;
            this.movementSpeed = movementSpeed;
            this.lockX = lockX;
            this.lockY = lockY;
        }

        internal void Update(GameTime gameTime, in int textureWidth, in int textureHeight)
        {
            float elapsedSeconds = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Vector2 cameraPosition = Camera.Position;

            if (!this.lockX)
            {
                if (this.movementSpeed.X != 0)
                {
                    this.movementOffsetPosition.X += this.movementSpeed.X * elapsedSeconds;
                }

                this.layerPosition.X = this.movementOffsetPosition.X + (this.parallaxFactor.X * cameraPosition.X * elapsedSeconds * -1);
                this.layerPosition.X %= textureWidth;
            }

            if (!this.lockY)
            {
                if (this.movementSpeed.Y != 0)
                {
                    this.movementOffsetPosition.Y += this.movementSpeed.Y * elapsedSeconds;
                }

                this.layerPosition.Y = this.movementOffsetPosition.Y + (this.parallaxFactor.Y * cameraPosition.Y * elapsedSeconds);
                this.layerPosition.Y %= textureHeight;
            }

            this.horizontalLayerPosition.X = this.layerPosition.X >= 0
                ? this.layerPosition.X - textureWidth
                : this.layerPosition.X + textureWidth;

            this.verticalLayerPosition.Y = this.layerPosition.Y >= 0
                ? this.layerPosition.Y - textureHeight
                : this.layerPosition.Y + textureHeight;

            this.horizontalLayerPosition.Y = this.layerPosition.Y;
            this.verticalLayerPosition.X = this.layerPosition.X;

            this.diagonalLayerPosition.X = this.horizontalLayerPosition.X;
            this.diagonalLayerPosition.Y = this.verticalLayerPosition.Y;
        }

        internal void Draw(SpriteBatch spriteBatch, in Texture2D texture)
        {
            void DrawLayer(SpriteBatch spriteBatch, in Texture2D texture, in Vector2 position)
            {
                spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }

            DrawLayer(spriteBatch, texture, this.layerPosition);

            if (!this.lockX)
            {
                DrawLayer(spriteBatch, texture, this.horizontalLayerPosition);
            }

            if (!this.lockY)
            {
                DrawLayer(spriteBatch, texture, this.verticalLayerPosition);
            }

            if (!this.lockX && !this.lockY)
            {
                DrawLayer(spriteBatch, texture, this.diagonalLayerPosition);
            }
        }
    }
}

