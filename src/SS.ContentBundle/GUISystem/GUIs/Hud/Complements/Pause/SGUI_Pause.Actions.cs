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
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.COLOR_PICKER_TOOL_IDENTIFIER);
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = true;
        }

        private void ExitButtonAction()
        {
            this.guiConfirm.Configure(this.exitConfirmSettings);
            this.SGameInstance.GUIManager.OpenGUI(this.guiConfirm.Identifier);
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = true;
        }
    }
}
