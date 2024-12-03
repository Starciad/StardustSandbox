using Microsoft.Xna.Framework;

using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_OptionsMenu
    {
        private ISGUILayoutBuilder layout;

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
            BuildBackgroundPanel();
            BuildLeftPanel();
            BuildRightPanel();
        }

        private void BuildBackgroundPanel()
        {
            SGUISliceImageElement explorerBackground = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new Vector2(32, 15),
                Margin = new Vector2(128, 128),
                Color = new Color(24, 13, 65, 255)
            };

            explorerBackground.PositionRelativeToScreen();

            this.layout.AddElement(explorerBackground);
        }

        private void BuildLeftPanel()
        {

        }

        private void BuildRightPanel()
        {

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
