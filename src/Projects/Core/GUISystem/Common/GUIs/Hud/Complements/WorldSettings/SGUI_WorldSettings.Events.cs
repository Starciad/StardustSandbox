namespace StardustSandbox.Core.GUISystem.GUIs.Hud.Complements.WorldSettings
{
    internal sealed partial class SGUI_WorldSettings
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
