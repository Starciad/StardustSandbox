using StardustSandbox.ContentBundle.Enums.GUISystem;
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

                this.inputSettings.OnSendCallback?.Invoke(argumentResult);
            }

            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
