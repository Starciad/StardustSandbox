using StardustSandbox.Core.IO.Handlers;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.WorldExplorer
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
            Array.Clear(this.savedWorldFilesLoaded);
        }

        private void LoadAllLocalSavedWorlds()
        {
            this.savedWorldFilesLoaded = SWorldSavingHandler.LoadAllSavedWorldData(this.SGameInstance.GraphicsManager.GraphicsDevice);
        }
    }
}
