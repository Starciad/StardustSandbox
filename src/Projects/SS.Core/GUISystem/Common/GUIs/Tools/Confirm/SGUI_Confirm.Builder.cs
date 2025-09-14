using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Common.Elements.Graphics;
using StardustSandbox.Core.GUISystem.Common.Elements.Textual;
using StardustSandbox.Core.GUISystem.Common.Helpers.Interactive;
using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.GameContent.GUISystem.GUIs.Tools.Confirm
{
    internal sealed partial class SGUI_Confirm
    {
        private SGUITextElement captionElement;
        private SGUITextElement messageElement;

        private readonly SGUILabelElement[] menuButtonElements;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildBackground(layoutBuilder);
            BuildCaption(layoutBuilder);
            BuildMessage(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
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

        private void BuildCaption(ISGUILayoutBuilder layoutBuilder)
        {
            this.captionElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(0, 96),
                LineHeight = 1.25f,
                TextAreaSize = new(850, 1000),
                SpriteFont = this.pixelOperatorSpriteFont,
                PositionAnchor = SCardinalDirection.North,
                OriginPivot = SCardinalDirection.Center,
            };

            this.captionElement.SetTextualContent("Caption");
            this.captionElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.captionElement);
        }

        private void BuildMessage(ISGUILayoutBuilder layoutBuilder)
        {
            this.messageElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(0, -128),
                LineHeight = 1.25f,
                TextAreaSize = new(850, 1000),
                SpriteFont = this.pixelOperatorSpriteFont,
                PositionAnchor = SCardinalDirection.Center,
                OriginPivot = SCardinalDirection.Center,
            };

            this.messageElement.SetTextualContent("Message");
            this.messageElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.messageElement);
        }

        private void BuildMenuButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 margin = new(0, -64);

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

                labelElement.SetTextualContent(button.Name);
                labelElement.PositionRelativeToScreen();
                labelElement.SetAllBorders(true, SColorPalette.DarkGray, new(2));

                margin.Y -= 72;

                layoutBuilder.AddElement(labelElement);

                this.menuButtonElements[i] = labelElement;
            }
        }
    }
}
