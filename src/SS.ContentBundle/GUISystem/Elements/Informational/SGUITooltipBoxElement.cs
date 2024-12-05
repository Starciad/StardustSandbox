using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.GUISystem.Elements.Informational
{
    public sealed class SGUITooltipBoxElement : SGUIElement
    {
        public bool IsShowing { get; private set; }

        private readonly SGUISliceImageElement backgroundImageElement;
        private readonly SGUILabelElement titleElement;
        private readonly SGUITextElement descriptionElement;

        private readonly SGUILayout tooltipLayout;

        public SGUITooltipBoxElement(ISGame gameInstance) : base(gameInstance)
        {
            this.IsVisible = true;
            this.ShouldUpdate = true;

            this.Margin = new(60f);

            this.backgroundImageElement = new(this.SGameInstance)
            {
                Texture = this.SGameInstance.AssetDatabase.GetTexture("gui_background_4"),
                Color = new(SColorPalette.DarkGray, 200),
                Scale = new(10f, 10f),
                PositionAnchor = SCardinalDirection.Center,
                Size = new(32f),
            };

            this.titleElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                SpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.ARIAL),
            };

            this.descriptionElement = new(this.SGameInstance)
            {
                Scale = new(0.05f),
                Margin = new(0, 16f),
                SpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.ARIAL),
            };

            this.tooltipLayout = new(gameInstance);
            this.tooltipLayout.AddElement(this.backgroundImageElement);

            this.tooltipLayout.AddElement(this.titleElement);
            this.tooltipLayout.AddElement(this.descriptionElement);
        }

        public override void Initialize()
        {
            this.tooltipLayout.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.tooltipLayout.Update(gameTime);

            Vector2 mousePosition = this.SGameInstance.InputManager.GetScaledMousePosition();
            Vector2 newPosition = mousePosition + this.Margin;

            if (newPosition.X + this.backgroundImageElement.Size.Width > SScreenConstants.DEFAULT_SCREEN_WIDTH)
            {
                newPosition.X = mousePosition.X - this.backgroundImageElement.Size.Width - this.Margin.X;
            }

            if (newPosition.Y + this.backgroundImageElement.Size.Height > SScreenConstants.DEFAULT_SCREEN_HEIGHT)
            {
                newPosition.Y = mousePosition.Y - this.backgroundImageElement.Size.Height - this.Margin.Y;
            }

            this.backgroundImageElement.Position = newPosition;

            this.titleElement.PositionRelativeToElement(this.backgroundImageElement);
            this.descriptionElement.PositionRelativeToElement(this.titleElement);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.tooltipLayout.Draw(gameTime, spriteBatch);
        }

        public void Show()
        {
            this.IsVisible = true;
            this.IsShowing = true;
        }

        public void Close()
        {
            this.IsVisible = false;
            this.IsShowing = false;
        }

        public void SetTitle(string value)
        {
            this.titleElement.SetTextualContent(value);
        }

        public void SetDescription(string value)
        {
            this.descriptionElement.SetTextualContent(value);
        }
    }
}
