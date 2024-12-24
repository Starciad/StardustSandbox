using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Specials
{
    internal sealed partial class SGUI_Input
    {
        private SGUITextElement synopsisElement;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildGUIBackground(layoutBuilder);
            BuildSynopsis(layoutBuilder);
            BuildInputTypingField(layoutBuilder);
        }

        private void BuildGUIBackground(ISGUILayoutBuilder layoutBuilder)
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

        private void BuildSynopsis(ISGUILayoutBuilder layoutBuilder)
        {
            this.synopsisElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(0, -160),
                LineHeight = 1.25f,
                TextAreaSize = new(600, 1000),
                SpriteFont = this.pixelOperatorSpriteFont,
                PositionAnchor = SCardinalDirection.Center,
                OriginPivot = SCardinalDirection.Center,
            };

            this.synopsisElement.SetTextualContent("Synopsis");
            this.synopsisElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.synopsisElement);
        }

        private void BuildInputTypingField(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement typingFieldElement = new(this.SGameInstance)
            {
                Texture = this.typingFieldTexture,
                Scale = new(1.5f),
                Size = new(632, 50),
                Margin = new(0, 64),
                PositionAnchor = SCardinalDirection.Center,
                OriginPivot = SCardinalDirection.Center,
            };

            typingFieldElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(typingFieldElement);
        }
    }
}
