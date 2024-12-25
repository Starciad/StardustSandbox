namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Specials
{
    internal sealed partial class SGUI_Input
    {
        private void CancelButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        private void SendButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
