namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.TextInput
{
    internal sealed partial class SGUI_Slider
    {
        private void CancelButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        private void SendButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
            this.sliderSettings?.OnSendCallback?.Invoke(new(this.currentValue));
        }
    }
}
