namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_Pause
    {
        // Menu
        private void ExitButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
