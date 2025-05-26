using StardustSandbox.Core.IO.Handlers;

namespace StardustSandbox.GameContent.GUISystem.GUIs.Menus.WorldExplorer
{
    internal sealed partial class SGUI_WorldExplorerMenu
    {
        protected override void OnOpened()
        {
            ReloadButtonAction();
            ChangeWorldsCatalog();
        }

        protected override void OnClosed()
        {
            this.savedWorldFilesLoaded.Clear();
        }

        private void LoadAllLocalSavedWorlds()
        {
            this.savedWorldFilesLoaded = [.. SWorldSavingHandler.LoadAllSavedWorldData(this.SGameInstance.GraphicsManager.GraphicsDevice)];
        }
    }
}
