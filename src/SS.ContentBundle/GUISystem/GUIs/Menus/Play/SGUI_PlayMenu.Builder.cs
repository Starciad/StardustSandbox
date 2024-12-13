using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_PlayMenu
    {
        private ISGUILayoutBuilder layout;

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this.layout = layout;

        }
    }
}
