namespace StardustSandbox.Core.GUISystem.GUIs.Hud.Complements.Information
{
    internal sealed partial class SGUI_Information
    {
        // Menu
        private void ExitButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
