using StardustSandbox.Core.Constants.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_PlayMenu
    {
        private void WorldsButtonAction()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.LOCAL_WORLDS_IDENTIFIER);
        }

        private void ReturnButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
