using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_CreditsMenu
    {
        private ISGUILayoutBuilder layout;

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this.layout = layout;
        }


    }
}
