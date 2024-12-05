using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_MainMenu
    {
        private ISGUILayoutBuilder layout;

        private SGUIImageElement panelBackgroundElement;
        private SGUIImageElement gameTitleElement;

        private readonly SGUILabelElement[] menuButtonElements = new SGUILabelElement[5];

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this.layout = layout;

            BuildMainPanel();
            BuildDecorations();
            BuildGameTitle();
            BuildButtons();
            BuildInfos();
        }

        private void BuildMainPanel()
        {
            this.panelBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new Vector2(487f, SScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = SSize2F.One,
                Color = new(SColorPalette.DarkGray, 180),
            };

            this.layout.AddElement(this.panelBackgroundElement);
        }

        private void BuildDecorations()
        {
            SGUIImageElement prosceniumCurtainElement = new(this.SGameInstance)
            {
                Texture = this.prosceniumCurtainTexture,
                Scale = new Vector2(2)
            };

            this.layout.AddElement(prosceniumCurtainElement);
        }

        private void BuildInfos()
        {
            SGUILabelElement gameVersionLabel = new(this.SGameInstance)
            {
                Margin = new Vector2(-32f, -32f),
                Scale = new Vector2(0.08f),
                Color = Color.White,
                PositionAnchor = SCardinalDirection.Southeast,
                OriginPivot = SCardinalDirection.West
            };

            SGUILabelElement copyrightLabel = new(this.SGameInstance)
            {
                Margin = new Vector2(0f, -32),
                Scale = new Vector2(0.08f),
                Color = Color.White,
                PositionAnchor = SCardinalDirection.South,
                OriginPivot = SCardinalDirection.Center
            };

            gameVersionLabel.SetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);
            gameVersionLabel.SetTextualContent($"Ver. {SGameConstants.VERSION}");
            gameVersionLabel.PositionRelativeToScreen();

            copyrightLabel.SetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);
            copyrightLabel.SetTextualContent($"(c) {SGameConstants.YEAR} {SGameConstants.AUTHOR}");
            copyrightLabel.PositionRelativeToScreen();

            this.layout.AddElement(gameVersionLabel);
            this.layout.AddElement(copyrightLabel);
        }

        private void BuildGameTitle()
        {
            this.gameTitleElement = new(this.SGameInstance)
            {
                Texture = this.gameTitleTexture,
                Scale = new Vector2(1.5f),
                Size = new SSize2(292, 112),
                Margin = new Vector2(0, 96),
                PositionAnchor = SCardinalDirection.North,
                OriginPivot = SCardinalDirection.Center
            };

            this.gameTitleElement.PositionRelativeToElement(this.panelBackgroundElement);
        }

        private void BuildButtons()
        {
            // BUTTONS
            Vector2 baseMargin = new(0, 0);

            // Labels
            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                SGUILabelElement labelElement = new(this.SGameInstance)
                {
                    Scale = new Vector2(0.15f),
                    Margin = baseMargin,
                    Color = new Color(206, 214, 237, 255),
                    PositionAnchor = SCardinalDirection.Center,
                    OriginPivot = SCardinalDirection.Center
                };

                labelElement.SetTextualContent(this.menuButtonNames[i]);
                labelElement.SetAllBorders(true, SColorPalette.DarkGray, new Vector2(4f));
                labelElement.SetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);
                labelElement.PositionRelativeToElement(this.panelBackgroundElement);

                this.menuButtonElements[i] = labelElement;
                baseMargin.Y += 75;
            }

            this.layout.AddElement(this.gameTitleElement);

            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                this.layout.AddElement(this.menuButtonElements[i]);
            }
        }
    }
}
