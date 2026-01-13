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

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.InputSystem;
using StardustSandbox.Managers;

using System;

namespace StardustSandbox.UI.Elements
{
    internal static class TooltipBoxContent
    {
        internal static string Title => title;
        internal static string Description => description;

        private static string title = string.Empty;
        private static string description = string.Empty;

        internal static void SetTitle(string value)
        {
            title = value;
        }

        internal static void SetDescription(string value)
        {
            description = value;
        }
    }

    internal sealed class TooltipBox : UIElement
    {
        internal Vector2 MinimumSize { get; set; }
        internal Vector2 MaximumSize { get; set; }

        private readonly SliceImage background;
        private readonly Label title;
        private readonly Text description;

        private readonly CursorManager cursorManager;

        internal TooltipBox(CursorManager cursorManager)
        {
            this.CanDraw = true;
            this.CanUpdate = true;

            this.cursorManager = cursorManager;

            this.Margin = new(60f);

            this.background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.ShapeSquares),
                Color = AAP64ColorPalette.DarkPurple,
                Alignment = UIDirection.Center,
                Size = new(48f),

                TileSize = new(16),
                Origin = new(0, 32),
            };

            this.title = new()
            {
                Scale = new(0.12f),
                SpriteFontIndex = SpriteFontIndex.DigitalDisco,
                Margin = new(0, 0f),
            };

            this.description = new()
            {
                Scale = new(0.078f),
                Margin = new(0, 64f),
                LineHeight = 1.25f,
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
            };

            this.background.AddChild(this.title);
            this.background.AddChild(this.description);

            AddChild(this.background);

            this.MinimumSize = new(48f, 48f);
            this.MaximumSize = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT);
        }

        protected override void OnInitialize()
        {
            return;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.title.TextContent = TooltipBoxContent.Title;
            this.description.TextContent = TooltipBoxContent.Description;

            UpdateSize();
            UpdatePosition();
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            return;
        }

        private void UpdateSize()
        {
            Vector2 titleSize = this.title.Size;
            Vector2 descriptionSize = this.description.Size;

            float finalWidth = Math.Max(this.MinimumSize.X, titleSize.X);
            float finalHeight = Math.Max(this.MinimumSize.Y, descriptionSize.Y + titleSize.Y + 10.0f);

            finalWidth = finalWidth > this.MaximumSize.X ? this.MaximumSize.X : finalWidth;
            finalHeight = finalHeight > this.MaximumSize.Y ? this.MaximumSize.Y : finalHeight;

            Vector2 finalSize = new(finalWidth, finalHeight);
            Vector2 finalTextAreaSize = new(finalWidth, descriptionSize.Y);

            this.description.TextAreaSize = finalTextAreaSize;

            this.background.Size = finalSize;
            this.background.TileScale = finalSize / this.background.TileSize.ToVector2();
        }

        private void UpdatePosition()
        {
            Vector2 spacing = this.cursorManager.Scale * 16.0f;

            Vector2 mousePosition = Input.GetScaledMousePosition();
            Vector2 newPosition = mousePosition + this.Margin + spacing;

            if ((newPosition.X + this.background.Size.X) > ScreenConstants.SCREEN_WIDTH)
            {
                newPosition.X = mousePosition.X - this.background.Size.X - this.Margin.X - spacing.X;
            }

            if ((newPosition.Y + this.background.Size.Y) > ScreenConstants.SCREEN_HEIGHT)
            {
                newPosition.Y = mousePosition.Y - this.background.Size.Y - this.Margin.Y - spacing.Y;
            }

            this.background.Position = newPosition;
        }
    }
}

