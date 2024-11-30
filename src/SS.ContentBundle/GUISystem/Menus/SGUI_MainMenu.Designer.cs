using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.Menus
{
    public sealed partial class SGUI_MainMenu
    {
        private enum SMainMenuButtonIndex : byte
        {
            Create = 0,
            Play = 1,
            Options = 2,
            Quit = 3
        }

        private ISGUILayoutBuilder layout;

        private (SGUISliceImageElement background, SGUILabelElement label)[] menuButtons;

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this.layout = layout;

            BuildMainPanel();
            BuildInfos();
        }

        private void BuildMainPanel()
        {
            #region Instantiating
            SGUIImageElement panelBackground = new(this.SGameInstance);
            SGUIImageElement gameTitle = new(this.SGameInstance);
            this.menuButtons = [
                (new(this.SGameInstance), new(this.SGameInstance)),
                (new(this.SGameInstance), new(this.SGameInstance)),
                (new(this.SGameInstance), new(this.SGameInstance)),
                (new(this.SGameInstance), new(this.SGameInstance))
            ];
            #endregion

            #region Configuring
            panelBackground.SetTexture(this.particleTexture);
            panelBackground.SetScale(new Vector2(487f, SScreenConstants.DEFAULT_SCREEN_HEIGHT));
            panelBackground.SetSize(SSize2F.One);
            panelBackground.SetColor(new(Color.Black, 180));
            panelBackground.PositionRelativeToScreen();

            gameTitle.SetTexture(this.gameTitleTexture);
            gameTitle.SetScale(new Vector2(1.5f));
            gameTitle.SetSize(new SSize2(292, 112));
            gameTitle.SetMargin(new Vector2(0, 96));
            gameTitle.SetPositionAnchor(SCardinalDirection.North);
            gameTitle.SetOriginPivot(SCardinalDirection.Center);
            gameTitle.PositionRelativeToElement(panelBackground);

            // BUTTONS
            // Backgrounds
            Vector2 baseMargin = new(0, 0);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                this.menuButtons[i].background.SetTexture(this.buttonBackgroundTexture);
                this.menuButtons[i].background.SetScale(new Vector2(8, 1f));
                this.menuButtons[i].background.SetSize(new SSize2(96, 96));
                this.menuButtons[i].background.SetMargin(baseMargin);
                this.menuButtons[i].background.SetColor(Color.White);
                this.menuButtons[i].background.SetPositionAnchor(SCardinalDirection.Center);
                this.menuButtons[i].background.PositionRelativeToElement(panelBackground);
                baseMargin.Y += 110;
            }

            // Labels
            this.menuButtons[(byte)SMainMenuButtonIndex.Create].label.SetTextContent("Create");
            this.menuButtons[(byte)SMainMenuButtonIndex.Play].label.SetTextContent("Play");
            this.menuButtons[(byte)SMainMenuButtonIndex.Options].label.SetTextContent("Options");
            this.menuButtons[(byte)SMainMenuButtonIndex.Quit].label.SetTextContent("Quit");

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                this.menuButtons[i].label.SetScale(new Vector2(0.15f));
                this.menuButtons[i].label.SetMargin(new Vector2(18, -16));
                this.menuButtons[i].label.SetColor(new Color(206, 214, 237, 255));
                this.menuButtons[i].label.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
                this.menuButtons[i].label.SetBorders(true);
                this.menuButtons[i].label.SetBordersColor(new Color(45, 53, 74, 255));
                this.menuButtons[i].label.SetBorderOffset(new Vector2(4.4f));
                this.menuButtons[i].label.SetPositionAnchor(SCardinalDirection.West);
                this.menuButtons[i].label.PositionRelativeToElement(this.menuButtons[i].background);
            }
            #endregion

            #region Adding
            this.layout.AddElement(panelBackground);
            this.layout.AddElement(gameTitle);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                this.layout.AddElement(this.menuButtons[i].background);
                this.layout.AddElement(this.menuButtons[i].label);
            }
            #endregion
        }

        private void BuildInfos()
        {
            SGUILabelElement gameVersionLabel = new(this.SGameInstance);
            SGUILabelElement copyrightLabel = new(this.SGameInstance);

            gameVersionLabel.SetTextContent($"Ver. {SGameConstants.VERSION}");
            gameVersionLabel.SetMargin(new Vector2(-32f, -32f));
            gameVersionLabel.SetScale(new Vector2(0.08f));
            gameVersionLabel.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
            gameVersionLabel.SetColor(Color.White);
            gameVersionLabel.SetPositionAnchor(SCardinalDirection.Southeast);
            gameVersionLabel.SetOriginPivot(SCardinalDirection.West);
            gameVersionLabel.PositionRelativeToScreen();

            copyrightLabel.SetTextContent($"(c) {SGameConstants.YEAR} {SGameConstants.AUTHOR}");
            copyrightLabel.SetMargin(new Vector2(0f, -32));
            copyrightLabel.SetScale(new Vector2(0.08f));
            copyrightLabel.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
            copyrightLabel.SetColor(Color.White);
            copyrightLabel.SetPositionAnchor(SCardinalDirection.South);
            copyrightLabel.SetOriginPivot(SCardinalDirection.Center);
            copyrightLabel.PositionRelativeToScreen();

            this.layout.AddElement(gameVersionLabel);
            this.layout.AddElement(copyrightLabel);
        }
    }
}
