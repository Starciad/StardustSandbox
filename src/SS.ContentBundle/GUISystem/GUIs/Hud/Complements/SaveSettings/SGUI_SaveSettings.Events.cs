namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_SaveSettings
    {
        protected override void OnOpened()
        {
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = true;
            UpdateInfos();
        }

        protected override void OnClosed()
        {
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = false;
            this.worldThumbnailTexture.Dispose();
            this.worldThumbnailTexture = null;
        }
    }
}
