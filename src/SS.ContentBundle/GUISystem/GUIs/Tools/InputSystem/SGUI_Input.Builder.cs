using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Specials
{
    internal sealed partial class SGUI_Input
    {
        private SGUITextElement synopsisElement;
        private SGUITextElement userInputElement;
        private SGUILabelElement characterCountElement;

        private SGUIImageElement userInputBackgroundElement;

        private readonly SGUILabelElement[] menuButtonElements;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildGUIBackground(layoutBuilder);
            BuildSynopsis(layoutBuilder);
            BuildUserInput(layoutBuilder);
            BuildCharacterCount(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
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
                Margin = new(0, -128),
                LineHeight = 1.25f,
                TextAreaSize = new(850, 1000),
                SpriteFont = this.pixelOperatorSpriteFont,
                PositionAnchor = SCardinalDirection.Center,
                OriginPivot = SCardinalDirection.Center,
            };

            this.synopsisElement.SetTextualContent("Synopsis");
            this.synopsisElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.synopsisElement);
        }

        private void BuildUserInput(ISGUILayoutBuilder layoutBuilder)
        {
            this.userInputBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.typingFieldTexture,
                Scale = new(1.5f),
                Size = new(632, 50),
                Margin = new(0, 64),
                PositionAnchor = SCardinalDirection.Center,
                OriginPivot = SCardinalDirection.Center,
            };

            this.userInputElement = new(this.SGameInstance)
            {
                SpriteFont = this.pixelOperatorSpriteFont,
                Scale = new(0.085f),
                TextAreaSize = new(1000, 1000),
                Margin = new(0, -32),
                PositionAnchor = SCardinalDirection.Center,
                OriginPivot = SCardinalDirection.Center,
            };

            this.userInputBackgroundElement.PositionRelativeToScreen();
            this.userInputElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.userInputBackgroundElement);
            layoutBuilder.AddElement(this.userInputElement);
        }

        private void BuildCharacterCount(ISGUILayoutBuilder layoutBuilder)
        {
            this.characterCountElement = new(this.SGameInstance)
            {
                SpriteFont = this.pixelOperatorSpriteFont,
                Scale = new(0.08f),
                Margin = new(-212, -16),
                PositionAnchor = SCardinalDirection.East,
                OriginPivot = SCardinalDirection.West,
            };

            this.characterCountElement.SetTextualContent("000/000");
            this.characterCountElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.characterCountElement);
        }

        private void BuildMenuButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 margin = new(0, -48);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SButton button = this.menuButtons[i];

                SGUILabelElement labelElement = new(this.SGameInstance)
                {
                    SpriteFont = this.bigApple3PMSpriteFont,
                    Scale = new(0.125f),
                    Margin = margin,
                    PositionAnchor = SCardinalDirection.South,
                    OriginPivot = SCardinalDirection.Center,
                };

                labelElement.SetTextualContent(button.DisplayName);
                labelElement.PositionRelativeToScreen();
                labelElement.SetAllBorders(true, SColorPalette.DarkGray, new(2));

                margin.Y -= 72;

                layoutBuilder.AddElement(labelElement);

                this.menuButtonElements[i] = labelElement;
            }
        }
    }
}
