using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_OptionsMenu
    {
        private ISGUILayoutBuilder layout;

        private SGUILabelElement titleLabel;

        private SGUISliceImageElement panelBackground;
        private SGUISliceImageElement leftPanelBackground;
        private SGUISliceImageElement rightPanelBackground;

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this.layout = layout;

            BuildPanels();
            BuildTitle();

            BuildSectionButtons();
            BuildSections();
        }

        private void BuildPanels()
        {
            BuildPanelBackground();
            BuildLeftPanel();
            BuildRightPanel();
        }

        private void BuildPanelBackground()
        {
            this.panelBackground = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new Vector2(32f, 15f),
                Size = new SSize2(32),
                Margin = new Vector2(128f),
                Color = SColorPalette.NavyBlue
            };

            this.panelBackground.PositionRelativeToScreen();

            this.layout.AddElement(this.panelBackground);
        }

        private void BuildLeftPanel()
        {
            this.leftPanelBackground = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new Vector2(9f, 13f),
                Margin = new Vector2(32f),
                Size = new SSize2(32),
                Color = SColorPalette.RoyalBlue
            };

            this.leftPanelBackground.PositionRelativeToElement(this.panelBackground);
            this.layout.AddElement(this.leftPanelBackground);
        }

        private void BuildRightPanel()
        {
            this.rightPanelBackground = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new Vector2(18.2f, 13f),
                Margin = new Vector2(90f, 0f),
                Size = new SSize2(32),
                PositionAnchor = SCardinalDirection.Northeast,
                Color = SColorPalette.RoyalBlue
            };

            this.rightPanelBackground.PositionRelativeToElement(this.leftPanelBackground);
            this.layout.AddElement(this.rightPanelBackground);
        }

        private void BuildTitle()
        {
            this.titleLabel = new(this.SGameInstance)
            {
                Scale = new Vector2(0.15f),
                Margin = new Vector2(0f, 52.5f),
                Color = new Color(206, 214, 237, 255),
                BorderOffset = new Vector2(4.4f),
                PositionAnchor = SCardinalDirection.North,
                OriginPivot = SCardinalDirection.Center
            };

            this.titleLabel.SetTextContent("OPTIONS");
            this.titleLabel.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
            this.titleLabel.SetBorders(true);
            this.titleLabel.SetBordersColor(SColorPalette.DarkGray);
            this.titleLabel.PositionRelativeToScreen();

            this.layout.AddElement(this.titleLabel);
        }

        private void BuildSectionButtons()
        {

        }

        private void BuildSections()
        {
            BuildGeneralSection();
            BuildInterfaceSection();
            BuildVideoSection();
            BuildVolumeSection();
            BuildCursorSection();
            BuildCuntrolsSection();
            BuildLanguageSection();
        }

        private void BuildGeneralSection()
        {

        }

        private void BuildInterfaceSection()
        {

        }

        private void BuildVideoSection()
        {

        }

        private void BuildVolumeSection()
        {

        }

        private void BuildCursorSection()
        {

        }

        private void BuildCuntrolsSection()
        {

        }

        private void BuildLanguageSection()
        {

        }
    }
}
