using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.Menus
{
    public sealed partial class SGUI_MainMenu
    {
        private enum SMainMenuButtonIndex : byte
        {
            Create = 0,
            Play = 1,
            Options = 2,
            Credits = 3,
            Quit = 4
        }

        private ISGUILayoutBuilder layout;

        private SGUIImageElement panelBackgroundElement;
        private SGUIImageElement gameTitleElement;

        private SGUILabelElement[] menuButtonElements;
        private Action[] menuButtonActions;

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
            this.panelBackgroundElement = new(this.SGameInstance);
            this.panelBackgroundElement.SetTexture(this.particleTexture);
            this.panelBackgroundElement.SetScale(new Vector2(487f, SScreenConstants.DEFAULT_SCREEN_HEIGHT));
            this.panelBackgroundElement.SetSize(SSize2F.One);
            this.panelBackgroundElement.SetColor(new(Color.Black, 180));
            this.panelBackgroundElement.PositionRelativeToScreen();

            this.layout.AddElement(this.panelBackgroundElement);
        }

        private void BuildDecorations()
        {
            SGUIImageElement prosceniumCurtainElement = new(this.SGameInstance);

            prosceniumCurtainElement.SetTexture(this.prosceniumCurtainTexture);
            prosceniumCurtainElement.SetScale(new Vector2(2));

            this.layout.AddElement(prosceniumCurtainElement);
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

        private void BuildGameTitle()
        {
            this.gameTitleElement = new(this.SGameInstance);
            this.gameTitleElement.SetTexture(this.gameTitleTexture);
            this.gameTitleElement.SetScale(new Vector2(1.5f));
            this.gameTitleElement.SetSize(new SSize2(292, 112));
            this.gameTitleElement.SetMargin(new Vector2(0, 96));
            this.gameTitleElement.SetPositionAnchor(SCardinalDirection.North);
            this.gameTitleElement.SetOriginPivot(SCardinalDirection.Center);
            this.gameTitleElement.PositionRelativeToElement(this.panelBackgroundElement);
        }

        private void BuildButtons()
        {
            this.menuButtonElements = [
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance)
            ];

            this.menuButtonActions = [
                CreateMenuButton,
                null,
                null,
                null,
                QuitMenuButton
            ];

            // BUTTONS
            Vector2 baseMargin = new(0, 0);

            // Labels
            this.menuButtonElements[(byte)SMainMenuButtonIndex.Create].SetTextContent("Create");
            this.menuButtonElements[(byte)SMainMenuButtonIndex.Play].SetTextContent("Play");
            this.menuButtonElements[(byte)SMainMenuButtonIndex.Options].SetTextContent("Options");
            this.menuButtonElements[(byte)SMainMenuButtonIndex.Credits].SetTextContent("Credits");
            this.menuButtonElements[(byte)SMainMenuButtonIndex.Quit].SetTextContent("Quit");

            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                SGUILabelElement labelElement = this.menuButtonElements[i];

                labelElement.SetScale(new Vector2(0.15f));
                labelElement.SetMargin(baseMargin);
                labelElement.SetColor(new Color(206, 214, 237, 255));
                labelElement.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
                labelElement.SetBorders(true);
                labelElement.SetBordersColor(Color.Black);
                labelElement.SetBorderOffset(new Vector2(4f));
                labelElement.SetPositionAnchor(SCardinalDirection.Center);
                labelElement.SetOriginPivot(SCardinalDirection.Center);
                labelElement.PositionRelativeToElement(this.panelBackgroundElement);

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
