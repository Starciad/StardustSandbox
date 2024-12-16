using StardustSandbox.Core.Managers.IO;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_WorldsExplorerMenu
    {
        protected override void OnOpened()
        {

        }

        protected override void OnClosed()
        {

        }

        private void LoadWorlds()
        {
            switch (this.worldLoadingType)
            {
                case SWorldLoadingType.Local:
                    LoadAllLocalWorlds();
                    break;

                default:
                    break;
            }
        }

        private void LoadAllLocalWorlds()
        {
            SWorldSavingManager.LoadAllSavedWorldData(this.SGameInstance.GraphicsManager.GraphicsDevice);
        }
    }
}
