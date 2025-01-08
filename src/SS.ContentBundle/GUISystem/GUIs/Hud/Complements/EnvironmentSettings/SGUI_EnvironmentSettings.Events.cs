namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.EnvironmentSettings
{
    internal sealed partial class SGUI_EnvironmentSettings
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
