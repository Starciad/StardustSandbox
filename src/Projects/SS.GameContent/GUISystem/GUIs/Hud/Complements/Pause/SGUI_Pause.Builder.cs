using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.GameContent.GUISystem.Elements.Graphics;
using StardustSandbox.GameContent.GUISystem.Elements.Textual;
using StardustSandbox.GameContent.GUISystem.Helpers.General;
using StardustSandbox.GameContent.GUISystem.Helpers.Interactive;
using StardustSandbox.GameContent.Localization.GUIs;

namespace StardustSandbox.GameContent.GUISystem.GUIs.Hud.Complements.Pause
{
    internal sealed partial class SGUI_Pause
    {
        private SGUIImageElement panelBackgroundElement;
        private SGUILabelElement menuTitleElement;

        private readonly SSlot[] menuButtonSlots;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildBackground(layoutBuilder);
            BuildTitle(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
        }

        private void BuildBackground(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement backgroundShadowElement = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = new(1),
                Color = new(SColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.panelBackgroundTexture,
                Size = new(542, 540),
                Margin = new(this.panelBackgroundTexture.Width / 2 * -1, 90),
                PositionAnchor = SCardinalDirection.North,
            };

            this.panelBackgroundElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(backgroundShadowElement);
            layoutBuilder.AddElement(this.panelBackgroundElement);
        }

        private void BuildTitle(ISGUILayoutBuilder layoutBuilder)
        {
            this.menuTitleElement = new(this.SGameInstance)
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new(0.12f),
                PositionAnchor = SCardinalDirection.North,
                OriginPivot = SCardinalDirection.Center,
                Margin = new(0f, 40f),
                Color = SColorPalette.White,
            };

            this.menuTitleElement.SetTextualContent(SLocalization_GUIs.HUD_Complements_Pause_Title);
            this.menuTitleElement.SetAllBorders(true, SColorPalette.DarkGray, new(3f));
            this.menuTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            layoutBuilder.AddElement(this.menuTitleElement);
        }

        private void BuildMenuButtons(ISGUILayoutBuilder layoutBuilder)
        {
            float marginY = 118f;

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SButton button = this.menuButtons[i];

                SGUIImageElement backgroundElement = new(this.SGameInstance)
                {
                    Texture = this.guiLargeButtonTexture,
                    Color = SColorPalette.PurpleGray,
                    Size = new(320, 80),
                    Margin = new(this.guiLargeButtonTexture.Width / 2 * -1, marginY),
                    PositionAnchor = SCardinalDirection.North,
                };

                SGUILabelElement labelElement = new(this.SGameInstance)
                {
                    Scale = new(0.1f),
                    Color = SColorPalette.White,
                    SpriteFont = this.bigApple3PMSpriteFont,
                    PositionAnchor = SCardinalDirection.Center,
                    OriginPivot = SCardinalDirection.Center
                };

                labelElement.SetTextualContent(button.Name);
                labelElement.SetAllBorders(true, SColorPalette.DarkGray, new(2));

                backgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                labelElement.PositionRelativeToElement(backgroundElement);

                layoutBuilder.AddElement(backgroundElement);
                layoutBuilder.AddElement(labelElement);

                this.menuButtonSlots[i] = new(backgroundElement, null, labelElement);

                marginY += backgroundElement.Size.Height + 32;
            }
        }
    }
}
