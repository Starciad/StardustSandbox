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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Managers;

using System;

namespace StardustSandbox.Core.UI.Elements
{
    internal static class TooltipBoxContent
    {
        internal static string Title => title;
        internal static string Description => description;

        private static string title = string.Empty;
        private static string description = string.Empty;

        internal static void SetTitle(string value)
        {
            title = value ?? string.Empty;
        }

        internal static void SetDescription(string value)
        {
            description = value ?? string.Empty;
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

            this.background = new SliceImage
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.ShapeSquares),
                Color = AAP64ColorPalette.DarkPurple,
                Alignment = UIDirection.Center,
                Size = new(48f),
                TileSize = new(16),
                Origin = new(0, 32)
            };

            this.title = new Label
            {
                Scale = new(0.12f),
                SpriteFontIndex = SpriteFontIndex.DigitalDisco,
                Margin = Vector2.Zero
            };

            this.description = new Text
            {
                Scale = new(0.078f),
                Margin = new(0f, 64f),
                LineHeight = 1.25f,
                SpriteFontIndex = SpriteFontIndex.PixelOperator
            };

            this.background.AddChild(this.title);
            this.background.AddChild(this.description);
            AddChild(this.background);

            this.MinimumSize = new(48f, 48f);
            this.MaximumSize = GameScreen.GetViewport();
        }

        protected override void OnInitialize()
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            bool visible = !GameParameters.HideTooltips;

            this.background.CanDraw = visible;
            this.title.CanDraw = visible;
            this.description.CanDraw = visible;

            this.title.TextContent = TooltipBoxContent.Title;
            this.description.TextContent = TooltipBoxContent.Description;

            UpdateSize();
            UpdatePosition();
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        private void UpdateSize()
        {
            // Determine available width based on text content
            float contentWidth = Math.Max(this.title.Size.X, this.description.Size.X);

            float finalWidth = Math.Clamp(
                contentWidth,
                this.MinimumSize.X,
                this.MaximumSize.X
            );

            // Apply text area constraints BEFORE measuring size
            this.description.TextAreaSize = new(
                finalWidth,
                this.MaximumSize.Y
            );

            // Re-read sizes after layout constraints
            Vector2 titleSize = this.title.Size;
            Vector2 descriptionSize = this.description.Size;

            float finalHeight = Math.Max(
                this.MinimumSize.Y,
                titleSize.Y + descriptionSize.Y + 10f
            );

            finalHeight = Math.Min(finalHeight, this.MaximumSize.Y);

            // Apply final background size
            Vector2 finalSize = new(finalWidth, finalHeight);
            this.background.Size = finalSize;
            this.background.TileScale = finalSize / this.background.TileSize.ToVector2();
        }

        private void UpdatePosition()
        {
            Vector2 mousePosition = InputEngine.GetMousePosition();
            Vector2 spacing = this.cursorManager.Scale * 16f;
            Vector2 position = mousePosition + this.Margin + spacing;

            Vector2 viewport = GameScreen.GetViewport();

            if (position.X + this.background.Size.X > viewport.X)
            {
                position.X = mousePosition.X - this.background.Size.X - this.Margin.X - spacing.X;
            }

            if (position.Y + this.background.Size.Y > viewport.Y)
            {
                position.Y = mousePosition.Y - this.background.Size.Y - this.Margin.Y - spacing.Y;
            }

            position.X = Math.Clamp(
                position.X,
                32f,
                viewport.X - this.background.Size.X - 32f
            );

            position.Y = Math.Clamp(
                position.Y,
                32f,
                viewport.Y - this.background.Size.Y - 32f
            );

            this.background.Position = position;
        }
    }
}
