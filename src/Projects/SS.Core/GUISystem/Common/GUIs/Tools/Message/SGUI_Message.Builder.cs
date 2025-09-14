using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Common.Elements.Graphics;
using StardustSandbox.Core.GUISystem.Common.Elements.Textual;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Localization.Statements;

namespace StardustSandbox.GameContent.GUISystem.GUIs.Tools.Message
{
    internal sealed partial class SGUI_Message
    {
        private SGUITextElement messageElement;
        private SGUILabelElement continueButtonElement;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildBackground(layoutBuilder);
            BuildMessage(layoutBuilder);
            BuildButton(layoutBuilder);
        }

        private void BuildBackground(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement guiBackground = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = SScreenConstants.DEFAULT_SCREEN_SIZE,
                Color = new(SColorPalette.DarkGray, 160)
            };

            layoutBuilder.AddElement(guiBackground);
        }

        private void BuildMessage(ISGUILayoutBuilder layoutBuilder)
        {
            this.messageElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(0f, 96f),
                LineHeight = 1.25f,
                TextAreaSize = new(850, 1000),
                SpriteFont = this.pixelOperatorSpriteFont,
                PositionAnchor = SCardinalDirection.North,
                OriginPivot = SCardinalDirection.Center,
            };

            this.messageElement.SetTextualContent("Message");
            this.messageElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.messageElement);
        }

        private void BuildButton(ISGUILayoutBuilder layoutBuilder)
        {
            this.continueButtonElement = new(this.SGameInstance)
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new(0.13f),
                Margin = new(0f, -96f),
                PositionAnchor = SCardinalDirection.South,
                OriginPivot = SCardinalDirection.Center,
            };

            this.continueButtonElement.SetTextualContent(SLocalization_Statements.Continue);
            this.continueButtonElement.PositionRelativeToScreen();
            this.continueButtonElement.SetAllBorders(true, SColorPalette.DarkGray, new(2));

            layoutBuilder.AddElement(this.continueButtonElement);
        }
    }
}
