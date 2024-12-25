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
            this.inputSettings?.OnSendCallback?.Invoke(new(this.userInputStringBuilder.ToString()));
            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
