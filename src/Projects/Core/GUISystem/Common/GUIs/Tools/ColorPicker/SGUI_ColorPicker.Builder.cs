using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Common.Elements.Graphics;
using StardustSandbox.Core.GUISystem.Common.Elements.Textual;
using StardustSandbox.Core.GUISystem.Common.Helpers.General;
using StardustSandbox.Core.GUISystem.Common.Helpers.Interactive;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Localization.GUIs;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.GUISystem.GUIs.Tools.ColorPicker
{
    internal sealed partial class SGUI_ColorPicker
    {
        private SGUITextElement captionElement;

        private readonly SGUILabelElement[] menuButtonElements;
        private readonly SColorSlot[] colorButtonElements;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildBackground(layoutBuilder);
            BuildCaption(layoutBuilder);
            BuildColorButtons(layoutBuilder);
            BuildMenuButtons(layoutBuilder);

            layoutBuilder.AddElement(this.tooltipBoxElement);
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

            this.captionElement.SetTextualContent(SLocalization_GUIs.Tools_ColorPicker_Title);
            this.captionElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.captionElement);
        }

        private void BuildColorButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 baseMargin = new(74, 192);
            Vector2 margin = baseMargin;

            SSize2 textureSize = new(40, 22);

            int buttonsPerRow = 12;

            int totalButtons = this.colorButtons.Length;
            int totalRows = (totalButtons + buttonsPerRow - 1) / buttonsPerRow;

            int index = 0;

            for (int row = 0; row < totalRows; row++)
            {
                for (int col = 0; col < buttonsPerRow; col++)
                {
                    if (index >= totalButtons)
                    {
                        break;
                    }

                    SColorButton colorButton = this.colorButtons[index];

                    SGUIImageElement backgroundElement = new(this.SGameInstance)
                    {
                        Texture = this.colorButtonTexture,
                        TextureClipArea = new(new(00, 00), textureSize.ToPoint()),
                        Scale = new(2f),
                        Size = textureSize,
                        Color = colorButton.Color,
                        Margin = margin,
                    };

                    SGUIImageElement borderElement = new(this.SGameInstance)
                    {
                        Texture = this.colorButtonTexture,
                        TextureClipArea = new(new(00, 22), textureSize.ToPoint()),
                        Scale = new(2f),
                        Size = textureSize,
                    };

                    backgroundElement.PositionRelativeToScreen();
                    borderElement.PositionRelativeToElement(backgroundElement);

                    layoutBuilder.AddElement(backgroundElement);
                    layoutBuilder.AddElement(borderElement);

                    this.colorButtonElements[index] = new(backgroundElement, borderElement);
                    index++;

                    margin.X += backgroundElement.Size.Width + 16f;
                }

                margin.X = baseMargin.X;
                margin.Y += (textureSize.Height * 2) + 16f;
            }
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
