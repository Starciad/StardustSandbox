using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.Elements.Informational
{
    internal sealed class SGUITooltipBoxElement : SGUIElement
    {
        internal bool IsShowing { get; private set; }
        internal SSize2F MinimumSize { get; set; }
        internal SSize2F MaximumSize { get; set; }

        internal SGUISliceImageElement BackgroundImageElement => this.backgroundImageElement;
        internal SGUILabelElement TitleElement => this.titleElement;
        internal SGUITextElement DescriptionElement => this.descriptionElement;

        private readonly SGUISliceImageElement backgroundImageElement;
        private readonly SGUILabelElement titleElement;
        private readonly SGUITextElement descriptionElement;

        private readonly SGUILayout tooltipLayout;

        internal SGUITooltipBoxElement(ISGame gameInstance) : base(gameInstance)
        {
            this.IsVisible = true;
            this.ShouldUpdate = true;

            this.Margin = new(60f);

            this.backgroundImageElement = new(this.SGameInstance)
            {
                Texture = this.SGameInstance.AssetDatabase.GetTexture("gui_background_4"),
                Color = SColorPalette.DarkPurple,
                Scale = new(10f, 10f),
                PositionAnchor = SCardinalDirection.Center,
                Size = new(32f),
            };

            this.titleElement = new(this.SGameInstance)
            {
                Scale = new(0.12f),
                SpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont("font_8"),
                Margin = new(0, -16f),
            };

            this.descriptionElement = new(this.SGameInstance)
            {
                Scale = new(0.078f),
                Margin = new(0, 64f),
                LineHeight = 1.25f,
                SpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont("font_9"),
            };

            this.tooltipLayout = new(gameInstance);
            this.tooltipLayout.AddElement(this.backgroundImageElement);
            this.tooltipLayout.AddElement(this.titleElement);
            this.tooltipLayout.AddElement(this.descriptionElement);

            this.MaximumSize = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT);
        }

        public override void Initialize()
        {
            this.tooltipLayout.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.tooltipLayout.Update(gameTime);

            UpdateSize();
            UpdatePosition();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.tooltipLayout.Draw(gameTime, spriteBatch);
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
            SSize2F titleSize = this.titleElement.GetStringSize();
            SSize2F descriptionSize = this.descriptionElement.GetStringSize();
            SSize2F backgroundBaseSize = new(32f, 32f);

            float finalWidth = Math.Max(this.MinimumSize.Width, titleSize.Width);
            float finalHeight = Math.Max(this.MinimumSize.Height, descriptionSize.Height + titleSize.Height + 10f);

            finalWidth = finalWidth > this.MaximumSize.Width ? this.MaximumSize.Width : finalWidth;
            finalHeight = finalHeight > this.MaximumSize.Height ? this.MaximumSize.Height : finalHeight;

            Vector2 finalScale = new(
                finalWidth / backgroundBaseSize.Width,
                finalHeight / backgroundBaseSize.Height
            );

            SSize2F finalTextAreaSize = new(finalWidth, descriptionSize.Height);

            this.descriptionElement.TextAreaSize = finalTextAreaSize;
            this.backgroundImageElement.Scale = finalScale;
        }

        private void UpdatePosition()
        {
            Vector2 spacing = this.SGameInstance.CursorManager.Scale * 16;

            Vector2 mousePosition = this.SGameInstance.InputManager.GetScaledMousePosition();
            Vector2 newPosition = mousePosition + this.Margin + spacing;

            if (newPosition.X + this.backgroundImageElement.Size.Width > SScreenConstants.DEFAULT_SCREEN_WIDTH)
            {
                newPosition.X = mousePosition.X - this.backgroundImageElement.Size.Width - this.Margin.X - spacing.X;
            }

            if (newPosition.Y + this.backgroundImageElement.Size.Height > SScreenConstants.DEFAULT_SCREEN_HEIGHT)
            {
                newPosition.Y = mousePosition.Y - this.backgroundImageElement.Size.Height - this.Margin.Y - spacing.Y;
            }

            this.backgroundImageElement.Position = newPosition;

            this.titleElement.PositionRelativeToElement(this.backgroundImageElement);
            this.descriptionElement.PositionRelativeToElement(this.titleElement);
        }
    }
}
