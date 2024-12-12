namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    public sealed partial class SGUI_ItemExplorer
    {
        protected override void OnOpened()
        {
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = true;
            SelectItemCatalog("powders", 0);
        }

        protected override void OnClosed()
        {
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = false;
        }
    }
}
