namespace StardustSandbox.GameContent.GUISystem.GUIs.Hud.Complements.PenSettings
{
    internal sealed partial class SGUI_PenSettings
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
