using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Managers;

using System;

namespace StardustSandbox.UI.Elements
{
    internal static class TooltipBoxContent
    {
        internal static string Title => title;
        internal static string Description => description;

        private static string title;
        private static string description;

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
        internal bool IsShowing
        {
            get => this.CanDraw; set => this.CanDraw = value;
        }
        internal Vector2 MinimumSize { get; set; }
        internal Vector2 MaximumSize { get; set; }

        private readonly SliceImage background;
        private readonly Label title;
        private readonly Text description;

        private readonly CursorManager cursorManager;
        private readonly InputManager inputManager;

        internal TooltipBox(CursorManager cursorManager, InputManager inputManager)
        {
            this.CanDraw = true;
            this.CanUpdate = true;

            this.cursorManager = cursorManager;
            this.inputManager = inputManager;

            this.Margin = new(60f);

            this.background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.ShapeSquares),
                Color = AAP64ColorPalette.DarkPurple,
                Alignment = CardinalDirection.Center,
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

        private void RefreshDisplay()
        {
            if (this.CanDraw)
            {
                this.title.TextContent = TooltipBoxContent.Title;
                this.description.TextContent = TooltipBoxContent.Description;

                if (!this.IsShowing)
                {
                    this.IsShowing = true;
                }
            }
            else
            {
                this.IsShowing = false;

                this.title.TextContent = string.Empty;
                this.description.TextContent = string.Empty;
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            UpdateSize();
            UpdatePosition();
            RefreshDisplay();
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

            Vector2 mousePosition = this.inputManager.GetScaledMousePosition();
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
