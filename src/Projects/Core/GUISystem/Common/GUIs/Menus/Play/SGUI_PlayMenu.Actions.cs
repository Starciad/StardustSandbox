using StardustSandbox.Core.Constants.GUISystem;

namespace StardustSandbox.Core.GUISystem.GUIs.Menus.Play
{
    internal sealed partial class SGUI_PlayMenu
    {
        private void WorldsButtonAction()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.WORLDS_EXPLORER_IDENTIFIER);
        }

        private void ReturnButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
