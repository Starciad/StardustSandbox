using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Common.Elements.Graphics;
using StardustSandbox.Core.GUISystem.Common.Elements.Textual;
using StardustSandbox.Core.GUISystem.Common.Helpers.Interactive;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.GUISystem.GUIs.Menus.Play
{
    internal sealed partial class SGUI_PlayMenu
    {
        private readonly SGUILabelElement[] menuButtonElements;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildTitle(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
        }

        private void BuildTitle(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement backgroundImage = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Color = new(SColorPalette.DarkGray, 196),
                Size = SSize2.One,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, 128f),
            };

            SGUILabelElement titleLabel = new(this.SGameInstance)
            {
                Scale = new(0.2f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.Center,
                OriginPivot = SCardinalDirection.Center,
            };

            titleLabel.SetTextualContent("Play Menu");
            titleLabel.SetAllBorders(true, SColorPalette.DarkGray, new(2f));
            titleLabel.PositionRelativeToElement(backgroundImage);

            layoutBuilder.AddElement(backgroundImage);
            layoutBuilder.AddElement(titleLabel);
        }

        private void BuildMenuButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 margin = Vector2.Zero;

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SButton button = this.menuButtons[i];

                SGUILabelElement buttonLabel = new(this.SGameInstance)
                {
                    Scale = new(0.15f),
                    Margin = margin,
                    SpriteFont = this.bigApple3PMSpriteFont,
                    PositionAnchor = SCardinalDirection.Center,
                    OriginPivot = SCardinalDirection.Center,
                };

                buttonLabel.SetTextualContent(button.Name);
                buttonLabel.SetAllBorders(true, SColorPalette.DarkGray, new(2f));

                SGUIImageElement buttonIcon = new(this.SGameInstance)
                {
                    Texture = button.IconTexture,
                    PositionAnchor = SCardinalDirection.West,
                    OriginPivot = SCardinalDirection.Center,
                    Margin = new((buttonLabel.GetStringSize().Width + (button.IconTexture.Width / 2f)) * -1, 0f),
                    Scale = new(2),
                };

                buttonLabel.PositionRelativeToScreen();
                buttonIcon.PositionRelativeToElement(buttonLabel);

                layoutBuilder.AddElement(buttonLabel);
                layoutBuilder.AddElement(buttonIcon);

                margin.Y += buttonLabel.GetStringSize().Height + 32f;

                this.menuButtonElements[i] = buttonLabel;
            }
        }
    }
}
