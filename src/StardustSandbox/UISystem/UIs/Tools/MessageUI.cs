using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;

namespace StardustSandbox.UISystem.UIs.Tools
{
    internal sealed class MessageUI(
        UIIndex index,
        UIManager uiManager
    ) : UI(index)
    {
        private TextUIElement messageElement;
        private LabelUIElement continueButtonElement;

        private readonly Texture2D particleTexture = AssetDatabase.GetTexture("texture_particle_1");
        private readonly SpriteFont bigApple3PMSpriteFont = AssetDatabase.GetSpriteFont("font_2");
        private readonly SpriteFont pixelOperatorSpriteFont = AssetDatabase.GetSpriteFont("font_9");

        private readonly UIManager uiManager = uiManager;

        #region BUILDER

        protected override void OnBuild(Layout layout)
        {
            BuildBackground(layout);
            BuildMessage(layout);
            BuildButton(layout);
        }

        private void BuildBackground(Layout layout)
        {
            ImageUIElement guiBackground = new()
            {
                Texture = this.particleTexture,
                Scale = new(ScreenConstants.DEFAULT_SCREEN_WIDTH, ScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = ScreenConstants.DEFAULT_SCREEN_SIZE.ToVector2(),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            layout.AddElement(guiBackground);
        }

        private void BuildMessage(Layout layout)
        {
            this.messageElement = new()
            {
                Scale = new(0.1f),
                Margin = new(0f, 96f),
                LineHeight = 1.25f,
                TextAreaSize = new(850, 1000),
                SpriteFont = this.pixelOperatorSpriteFont,
                PositionAnchor = CardinalDirection.North,
                OriginPivot = CardinalDirection.Center,
            };

            this.messageElement.SetTextualContent("Message");
            this.messageElement.PositionRelativeToScreen();

            layout.AddElement(this.messageElement);
        }

        private void BuildButton(Layout layout)
        {
            this.continueButtonElement = new()
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new(0.13f),
                Margin = new(0f, -96f),
                PositionAnchor = CardinalDirection.South,
                OriginPivot = CardinalDirection.Center,
            };

            this.continueButtonElement.SetTextualContent(Localization_Statements.Continue);
            this.continueButtonElement.PositionRelativeToScreen();
            this.continueButtonElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2));

            layout.AddElement(this.continueButtonElement);
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateContinueButton();
        }

        private void UpdateContinueButton()
        {
            Vector2 position = this.continueButtonElement.Position;
            Vector2 size = this.continueButtonElement.GetStringSize() / 2;

            if (UIInteraction.OnMouseClick(position, size))
            {
                this.uiManager.CloseGUI();
            }

            this.continueButtonElement.Color = UIInteraction.OnMouseOver(position, size) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
        }

        internal void SetContent(string text)
        {
            this.messageElement.SetTextualContent(text);
        }

        #endregion
    }
}
