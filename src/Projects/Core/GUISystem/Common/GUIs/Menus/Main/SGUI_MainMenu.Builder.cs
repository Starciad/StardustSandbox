using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Common.Elements.Graphics;
using StardustSandbox.Core.GUISystem.Common.Elements.Textual;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.GUISystem.GUIs.Menus.Main
{
    internal sealed partial class SGUI_MainMenu
    {
        private SGUIImageElement panelBackgroundElement;
        private SGUIImageElement gameTitleElement;

        private readonly SGUILabelElement[] menuButtonElements;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildMainPanel(layoutBuilder);
            BuildDecorations(layoutBuilder);
            BuildGameTitle(layoutBuilder);
            BuildButtons(layoutBuilder);
            BuildInfos(layoutBuilder);
        }

        private void BuildMainPanel(ISGUILayoutBuilder layoutBuilder)
        {
            this.panelBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(487f, SScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = SSize2F.One,
                Color = new(SColorPalette.DarkGray, 180),
            };

            layoutBuilder.AddElement(this.panelBackgroundElement);
        }

        private void BuildDecorations(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement prosceniumCurtainElement = new(this.SGameInstance)
            {
                Texture = this.prosceniumCurtainTexture,
                Scale = new(2)
            };

            layoutBuilder.AddElement(prosceniumCurtainElement);
        }

        private void BuildInfos(ISGUILayoutBuilder layoutBuilder)
        {
            SGUILabelElement gameVersionLabel = new(this.SGameInstance)
            {
                Margin = new(-32f, -32f),
                Scale = new(0.08f),
                Color = SColorPalette.White,
                PositionAnchor = SCardinalDirection.Southeast,
                OriginPivot = SCardinalDirection.West,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            SGUILabelElement copyrightLabel = new(this.SGameInstance)
            {
                Margin = new(0f, -32),
                Scale = new(0.08f),
                Color = SColorPalette.White,
                PositionAnchor = SCardinalDirection.South,
                OriginPivot = SCardinalDirection.Center,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            gameVersionLabel.SetTextualContent($"Ver. {SGameConstants.VERSION}");
            gameVersionLabel.PositionRelativeToScreen();

            copyrightLabel.SetTextualContent($"(c) {SGameConstants.YEAR} {SGameConstants.AUTHOR}");
            copyrightLabel.PositionRelativeToScreen();

            layoutBuilder.AddElement(gameVersionLabel);
            layoutBuilder.AddElement(copyrightLabel);
        }

        private void BuildGameTitle(ISGUILayoutBuilder layoutBuilder)
        {
            this.gameTitleElement = new(this.SGameInstance)
            {
                Texture = this.gameTitleTexture,
                Scale = new(1.5f),
                Size = new(292, 112),
                Margin = new(0, 96),
                PositionAnchor = SCardinalDirection.North,
                OriginPivot = SCardinalDirection.Center
            };

            this.gameTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            layoutBuilder.AddElement(this.gameTitleElement);
        }

        private void BuildButtons(ISGUILayoutBuilder layoutBuilder)
        {
            // BUTTONS
            Vector2 margin = new(0, 0);

            // Labels
            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                SGUILabelElement labelElement = new(this.SGameInstance)
                {
                    Scale = new(0.15f),
                    Margin = margin,
                    Color = SColorPalette.White,
                    PositionAnchor = SCardinalDirection.Center,
                    OriginPivot = SCardinalDirection.Center,
                    SpriteFont = this.bigApple3PMSpriteFont,
                };

                labelElement.SetTextualContent(this.menuButtons[i].Name);
                labelElement.SetAllBorders(true, SColorPalette.DarkGray, new(4f));
                labelElement.PositionRelativeToElement(this.panelBackgroundElement);

                this.menuButtonElements[i] = labelElement;
                margin.Y += 75;
            }

            layoutBuilder.AddElement(this.gameTitleElement);

            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                layoutBuilder.AddElement(this.menuButtonElements[i]);
            }
        }
    }
}
