namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.Pause
{
    internal sealed partial class SGUI_Pause
    {
        protected override void OnOpened()
        {
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = true;
        }

        protected override void OnClosed()
        {
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = false;
        }
    }
}
