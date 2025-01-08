namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.ItemExplorer
{
    internal sealed partial class SGUI_ItemExplorer
    {
        // Menu
        private void ExitButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        // Pagination
        private void PreviousButtonAction()
        {
            if (this.currentPage > 0)
            {
                this.currentPage--;
            }
            else
            {
                this.currentPage = this.totalPages;
            }

            ChangeItemCatalog();
        }

        private void NextButtonAction()
        {
            if (this.currentPage < this.totalPages)
            {
                this.currentPage++;
            }
            else
            {
                this.currentPage = 0;
            }

            ChangeItemCatalog();
        }
    }
}
