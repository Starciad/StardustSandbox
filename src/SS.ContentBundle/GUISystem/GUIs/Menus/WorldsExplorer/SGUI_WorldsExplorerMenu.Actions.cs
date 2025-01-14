using StardustSandbox.Core.IO;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.WorldsExplorer
{
    internal sealed partial class SGUI_WorldsExplorerMenu
    {
        private void ReloadButtonAction()
        {
            LoadAllLocalSavedWorlds();
            this.currentPage = 0;
            UpdatePagination();
            ChangeWorldsCatalog();
        }

        private void ExitButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        private void PreviousButtonAction()
        {
            if (this.currentPage > 0)
            {
                this.currentPage--;
            }
            else
            {
                this.currentPage = this.totalPages - 1;
            }

            ChangeWorldsCatalog();
        }

        private void NextButtonAction()
        {
            if (this.currentPage < this.totalPages - 1)
            {
                this.currentPage++;
            }
            else
            {
                this.currentPage = 0;
            }

            ChangeWorldsCatalog();
        }

        private void OpenDirectoryInExplorerAction()
        {
            SDirectory.OpenDirectoryInFileExplorer(SDirectory.Worlds);
        }
    }
}
