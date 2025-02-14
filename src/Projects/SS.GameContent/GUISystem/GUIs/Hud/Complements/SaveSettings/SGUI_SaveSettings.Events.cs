namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.SaveSettings
{
    internal sealed partial class SGUI_SaveSettings
    {
        protected override void OnOpened()
        {
            if (string.IsNullOrWhiteSpace(this.world.Infos.Name))
            {
                this.world.Infos.Name = SLocalization_Statements.Untitled;
            }

            if (string.IsNullOrWhiteSpace(this.world.Infos.Description))
            {
                this.world.Infos.Description = SLocalization_Messages.NoDescription;
            }

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
