using StardustSandbox.ContentBundle.Enums.GUISystem.Tools.Confirm;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Tools
{
    internal sealed partial class SGUI_Confirm
    {
        private void CancelButtonAction()
        {
            this.confirmSettings?.OnConfirmCallback?.Invoke(SConfirmStatus.Cancelled);
            this.SGameInstance.GUIManager.CloseGUI();
        }

        private void ConfirmButtonAction()
        {
            this.confirmSettings?.OnConfirmCallback?.Invoke(SConfirmStatus.Confirmed);
            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
