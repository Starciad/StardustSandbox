namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_PenSettings
    {
        protected override void OnOpened()
        {
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = true;
        }

        protected override void OnClosed()
        {
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = false;
        }
    }
}
