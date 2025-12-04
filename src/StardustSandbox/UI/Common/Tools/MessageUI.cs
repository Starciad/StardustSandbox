using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UI;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;

namespace StardustSandbox.UI.Common.Tools
{
    internal sealed class MessageUI(
        UIIndex index,
        UIManager uiManager
    ) : UIBase(index)
    {
        private Text message;
        private Label continueButtonLabel;

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
            Image background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            root.AddChild(background);
        }

        private void BuildMessage(Container root)
        {
            this.message = new()
            {
                Scale = new(0.1f),
                Margin = new(0.0f, 96.0f),
                LineHeight = 1.25f,
                TextAreaSize = new(850.0f, 1000.0f),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = CardinalDirection.North,
            };

            root.AddChild(this.message);
        }

        private void BuildButton(Container root)
        {
            this.continueButtonLabel = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.13f),
                Margin = new(0.0f, -96.0f),
                Alignment = CardinalDirection.South,
                TextContent = Localization_Statements.Continue,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            root.AddChild(this.continueButtonLabel);
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            if (Interaction.OnMouseLeftClick(this.continueButtonLabel))
            {
                this.uiManager.CloseGUI();
            }

            this.continueButtonLabel.Color = Interaction.OnMouseOver(this.continueButtonLabel) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;

            base.Update(gameTime);
        }

        internal void SetContent(string text)
        {
            this.message.TextContent = text;
        }

        #endregion
    }
}
