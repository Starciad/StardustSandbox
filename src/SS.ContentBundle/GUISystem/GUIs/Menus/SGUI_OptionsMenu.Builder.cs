using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_OptionsMenu
    {
        private ISGUILayoutBuilder layout;

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
                Scale = new Vector2(16f, 7.5f),
                Margin = new Vector2(-32f, 32f),
                Size = new SSize2(32),
                PositionAnchor = SCardinalDirection.Northeast,
                Color = SColorPalette.RoyalBlue
            };

            this.rightPanelBackground.PositionRelativeToElement(this.panelBackground);
            this.layout.AddElement(this.rightPanelBackground);
        }

        private void BuildTitle()
        {

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
