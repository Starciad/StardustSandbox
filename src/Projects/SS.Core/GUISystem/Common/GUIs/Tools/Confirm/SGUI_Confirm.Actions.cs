using StardustSandbox.Core.Enums.GUISystem.Tools.Confirm;

namespace StardustSandbox.GameContent.GUISystem.GUIs.Tools.Confirm
{
    internal sealed partial class SGUI_Confirm
    {
        private void CancelButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
            this.confirmSettings?.OnConfirmCallback?.Invoke(SConfirmStatus.Cancelled);
        }

        private void ConfirmButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
            this.confirmSettings?.OnConfirmCallback?.Invoke(SConfirmStatus.Confirmed);
        }
    }
}
