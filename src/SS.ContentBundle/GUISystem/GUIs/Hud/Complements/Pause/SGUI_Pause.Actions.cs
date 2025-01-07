using StardustSandbox.Core.Constants.GUISystem;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_Pause
    {
        private void ResumeButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        private void OptionsButtonAction()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.OPTIONS_MENU_IDENTIFIER);
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = true;
        }

        private void ExitButtonAction()
        {
            this.SGameInstance.GUIManager.Reset();
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.MAIN_MENU_IDENTIFIER);
        }
    }
}
