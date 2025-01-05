using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.InputSystem.Settings;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Specials
{
    internal sealed partial class SGUI_Input
    {
        private void CancelButtonAction()
        {
            this.inputSettings?.OnCancelCallback?.Invoke();
            this.SGameInstance.GUIManager.CloseGUI();
        }

        private void SendButtonAction()
        {
            if (this.inputSettings != null)
            {
                SArgumentResult argumentResult = new(this.userInputStringBuilder.ToString());

                if (string.IsNullOrWhiteSpace(argumentResult.Content))
                {
                    goto FINALIZATION;
                }

                this.inputSettings.OnSendCallback?.Invoke(argumentResult);
            }

        FINALIZATION:
            ;
            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
