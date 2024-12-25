using StardustSandbox.Core.Managers.IO;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_WorldsExplorerMenu
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
            this.savedWorldFilesLoaded = SWorldSavingManager.LoadAllSavedWorldData(this.SGameInstance.GraphicsManager.GraphicsDevice);
        }
    }
}
