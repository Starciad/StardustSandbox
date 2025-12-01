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
        internal static string Title { get; set; }
        internal static string Description { get; set; }
    }

    internal sealed class TooltipBox : UIElement
    {
        internal bool IsShowing
        {
            get => this.CanDraw; set => this.CanDraw = value;
        }
        internal Vector2 MinimumSize { get; set; }
        internal Vector2 MaximumSize { get; set; }

        internal SliceImage Background => this.background;
        internal Label Title => this.title;
        internal Text Description => this.description;

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
                Scale = new(10f, 10f),
                Alignment = CardinalDirection.Center,
                Size = new(32f),

                TileSize = new(16),
                SourceRectangle = new(0, 32, 48, 48),
            };

            this.title = new()
            {
                Scale = new(0.12f),
                SpriteFontIndex = SpriteFontIndex.DigitalDisco,
                Margin = new(0, -16f),
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

            this.MaximumSize = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT);
        }

        internal override void Initialize()
        {
            base.Initialize();
        }

        internal override void Update(GameTime gameTime)
        {
            UpdateSize();
            UpdatePosition();

            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        internal void RefreshDisplay(string title, string description)
        {
            if (this.CanDraw)
            {
                this.title.TextContent = title;
                this.description.TextContent = description;

                UpdateSize();
                UpdatePosition();

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

        internal void SetTitle(string value)
        {
            this.title.TextContent = value;

            UpdateSize();
            UpdatePosition();
        }

        internal void SetDescription(string value)
        {
            this.description.TextContent = value;

            UpdateSize();
            UpdatePosition();
        }

        private void UpdateSize()
        {
            Vector2 titleSize = this.title.MeasuredText;
            Vector2 descriptionSize = this.description.MeasuredText;
            Vector2 backgroundBaseSize = new(32f, 32f);

            float finalWidth = Math.Max(this.MinimumSize.X, titleSize.X);
            float finalHeight = Math.Max(this.MinimumSize.Y, descriptionSize.Y + titleSize.Y + 10f);

            finalWidth = finalWidth > this.MaximumSize.X ? this.MaximumSize.X : finalWidth;
            finalHeight = finalHeight > this.MaximumSize.Y ? this.MaximumSize.Y : finalHeight;

            Vector2 finalScale = new(
                finalWidth / backgroundBaseSize.X,
                finalHeight / backgroundBaseSize.Y
            );

            Vector2 finalTextAreaSize = new(finalWidth, descriptionSize.Y);

            this.description.TextAreaSize = finalTextAreaSize;
            this.background.Scale = finalScale;
        }

        private void UpdatePosition()
        {
            Vector2 spacing = this.cursorManager.Scale * 16;

            Vector2 mousePosition = this.inputManager.GetScaledMousePosition();
            Vector2 newPosition = mousePosition + this.Margin + spacing;

            if (newPosition.X + this.background.Size.X > ScreenConstants.SCREEN_WIDTH)
            {
                newPosition.X = mousePosition.X - this.background.Size.X - this.Margin.X - spacing.X;
            }

            if (newPosition.Y + this.background.Size.Y > ScreenConstants.SCREEN_HEIGHT)
            {
                newPosition.Y = mousePosition.Y - this.background.Size.Y - this.Margin.Y - spacing.Y;
            }

            this.background.Position = newPosition;
        }
    }
}
