using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;

using System;

namespace StardustSandbox.UISystem.Elements
{
    internal sealed class TooltipBoxUIElement : UIElement
    {
        internal bool IsShowing { get; private set; }
        internal Vector2 MinimumSize { get; set; }
        internal Vector2 MaximumSize { get; set; }

        internal SliceImageUIElement BackgroundImageElement => this.backgroundImageElement;
        internal LabelUIElement TitleElement => this.titleElement;
        internal TextUIElement DescriptionElement => this.descriptionElement;

        private readonly SliceImageUIElement backgroundImageElement;
        private readonly LabelUIElement titleElement;
        private readonly TextUIElement descriptionElement;

        private readonly Layout tooltipLayout;

        private readonly CursorManager cursorManager;
        private readonly InputManager inputManager;

        internal TooltipBoxUIElement(CursorManager cursorManager, InputManager inputManager)
        {
            this.cursorManager = cursorManager;
            this.inputManager = inputManager;

            this.IsVisible = true;
            this.ShouldUpdate = true;

            this.Margin = new(60f);

            this.backgroundImageElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.ShapeSquares),
                TextureClipArea = new(0, 0, 46, 46),
                Color = AAP64ColorPalette.DarkPurple,
                Scale = new(10f, 10f),
                PositionAnchor = CardinalDirection.Center,
                Size = new(32f),
            };

            this.titleElement = new()
            {
                Scale = new(0.12f),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.DigitalDisco),
                Margin = new(0, -16f),
            };

            this.descriptionElement = new()
            {
                Scale = new(0.078f),
                Margin = new(0, 64f),
                LineHeight = 1.25f,
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.PixelOperator),
            };

            this.tooltipLayout = new();
            this.tooltipLayout.AddElement(this.backgroundImageElement);
            this.tooltipLayout.AddElement(this.titleElement);
            this.tooltipLayout.AddElement(this.descriptionElement);

            this.MaximumSize = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT);
        }

        internal override void Initialize()
        {
            this.tooltipLayout.Initialize();
        }

        internal override void Update(GameTime gameTime)
        {
            this.tooltipLayout.Update(gameTime);

            UpdateSize();
            UpdatePosition();
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            this.tooltipLayout.Draw(spriteBatch);
        }

        internal void RefreshDisplay(string title, string description)
        {
            if (this.IsVisible)
            {
                this.titleElement.SetTextualContent(title);
                this.descriptionElement.SetTextualContent(description);

                UpdateSize();
                UpdatePosition();

                if (!this.IsShowing)
                {
                    Show();
                }
            }
            else
            {
                Hide();

                this.titleElement.ClearTextualContent();
                this.descriptionElement.ClearTextualContent();
            }
        }

        internal void Show()
        {
            this.IsVisible = true;
            this.IsShowing = true;
        }

        internal void Hide()
        {
            this.IsVisible = false;
            this.IsShowing = false;
        }

        internal void SetTitle(string value)
        {
            this.titleElement.SetTextualContent(value);

            UpdateSize();
            UpdatePosition();
        }

        internal void SetDescription(string value)
        {
            this.descriptionElement.SetTextualContent(value);

            UpdateSize();
            UpdatePosition();
        }

        private void UpdateSize()
        {
            Vector2 titleSize = this.titleElement.GetStringSize();
            Vector2 descriptionSize = this.descriptionElement.GetStringSize();
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

            this.descriptionElement.TextAreaSize = finalTextAreaSize;
            this.backgroundImageElement.Scale = finalScale;
        }

        private void UpdatePosition()
        {
            Vector2 spacing = this.cursorManager.Scale * 16;

            Vector2 mousePosition = this.inputManager.GetScaledMousePosition();
            Vector2 newPosition = mousePosition + this.Margin + spacing;

            if (newPosition.X + this.backgroundImageElement.Size.X > ScreenConstants.SCREEN_WIDTH)
            {
                newPosition.X = mousePosition.X - this.backgroundImageElement.Size.X - this.Margin.X - spacing.X;
            }

            if (newPosition.Y + this.backgroundImageElement.Size.Y > ScreenConstants.SCREEN_HEIGHT)
            {
                newPosition.Y = mousePosition.Y - this.backgroundImageElement.Size.Y - this.Margin.Y - spacing.Y;
            }

            this.backgroundImageElement.Position = newPosition;

            this.titleElement.PositionRelativeToElement(this.backgroundImageElement);
            this.descriptionElement.PositionRelativeToElement(this.titleElement);
        }
    }
}
