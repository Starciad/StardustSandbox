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

using StardustSandbox.Core.Cameras;
using StardustSandbox.Core.Enums.Backgrounds;

using System;

namespace StardustSandbox.Core.Backgrounds
{
    internal sealed class BackgroundLayer
    {
        internal BackgroundAnchoring Anchoring { get; init; }
        internal Vector2 AnchoringOffset { get; init; }
        internal Vector2 AutoMovementSpeed { get; init; }
        internal bool IsAffectedByLighting { get; init; }
        internal bool IsFixedHorizontally { get; init; }
        internal bool IsFixedVertically { get; init; }
        internal Vector2 ParallaxSpeed { get; init; }
        internal bool RepeatHorizontally { get; init; }
        internal bool RepeatVertically { get; init; }
        internal Texture2D Texture { get; init; }
        internal Rectangle TextureSourceRectangle { get; init; }

        private Vector2 layerPosition = Vector2.Zero;

        private static Vector2 GetAnchorNormalized(BackgroundAnchoring anchoring)
        {
            return anchoring switch
            {
                BackgroundAnchoring.Northwest => new(0f, 0f),
                BackgroundAnchoring.North => new(0.5f, 0f),
                BackgroundAnchoring.Northeast => new(1f, 0f),
                BackgroundAnchoring.West => new(0f, 0.5f),
                BackgroundAnchoring.Center => new(0.5f, 0.5f),
                BackgroundAnchoring.East => new(1f, 0.5f),
                BackgroundAnchoring.Southwest => new(0f, 1f),
                BackgroundAnchoring.South => new(0.5f, 1f),
                BackgroundAnchoring.Southeast => new(1f, 1f),
                _ => new(0.5f, 0.5f),
            };
        }

        internal void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.layerPosition += this.AutoMovementSpeed * elapsedSeconds;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (this.Texture == null)
            {
                return;
            }

            int width = this.TextureSourceRectangle.Width;
            int height = this.TextureSourceRectangle.Height;

            if (width <= 0 || height <= 0)
            {
                return;
            }

            Vector2 viewport = GameScreen.GetViewport();
            Vector2 cameraEffect = Camera.Position * this.ParallaxSpeed;

            if (this.IsFixedHorizontally)
            {
                cameraEffect.X = 0f;
            }

            if (this.IsFixedVertically)
            {
                cameraEffect.Y = 0f;
            }

            Vector2 anchorNormalized = GetAnchorNormalized(this.Anchoring);
            Vector2 anchorScreen = new(
                anchorNormalized.X * viewport.X,
                anchorNormalized.Y * viewport.Y
            );

            Vector2 basePosition = anchorScreen + this.AnchoringOffset + this.layerPosition - cameraEffect;

            int columns = 1;
            int rows = 1;

            if (this.RepeatHorizontally)
            {
                columns = (int)MathF.Ceiling(viewport.X / width) + 2;
            }

            if (this.RepeatVertically)
            {
                rows = (int)MathF.Ceiling(viewport.Y / height) + 2;
            }

            float startX = basePosition.X;
            float startY = basePosition.Y;

            if (this.RepeatHorizontally)
            {
                startX = (((basePosition.X % width) + width) % width) - width;
            }

            if (this.RepeatVertically)
            {
                startY = (((basePosition.Y % height) + height) % height) - height;
            }

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    Vector2 position = new(
                        startX + (x * width),
                        startY + (y * height)
                    );

                    spriteBatch.Draw(
                        this.Texture,
                        position,
                        this.TextureSourceRectangle,
                        Color.White
                    );
                }
            }
        }
    }
}
