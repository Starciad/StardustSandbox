using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UI;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;

namespace StardustSandbox.UI.Common.Tools
{
    internal sealed class MessageUI(
        UIIndex index,
        UIManager uiManager
    ) : UIBase(index)
    {
        private Text messageElement;
        private Label continueButtonElement;

        private readonly UIManager uiManager = uiManager;

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildMessage(root);
            BuildButton(root);
        }

        private static void BuildBackground(Container root)
        {
            Image guiBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = ScreenConstants.SCREEN_DIMENSIONS.ToVector2(),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            root.AddChild(guiBackground);
        }

        private void BuildMessage(Container root)
        {
            this.messageElement = new()
            {
                Scale = new(0.1f),
                Margin = new(0f, 96f),
                LineHeight = 1.25f,
                TextAreaSize = new(850, 1000),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = CardinalDirection.North,
                TextContent = "Message"
            };

            root.AddChild(this.messageElement);
        }

        private void BuildButton(Container root)
        {
            this.continueButtonElement = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.13f),
                Margin = new(0f, -96f),
                Alignment = CardinalDirection.South,
                TextContent = Localization_Statements.Continue,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2f,
                BorderThickness = 2f,
            };

            root.AddChild(this.continueButtonElement);
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
            Vector2 size = this.continueButtonElement.MeasuredText / 2;

            if (Interaction.OnMouseClick(position, size))
            {
                this.uiManager.CloseGUI();
            }

            this.continueButtonElement.Color = Interaction.OnMouseOver(position, size) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
        }

        internal void SetContent(string text)
        {
            this.messageElement.TextContent = text;
        }

        #endregion
    }
}
