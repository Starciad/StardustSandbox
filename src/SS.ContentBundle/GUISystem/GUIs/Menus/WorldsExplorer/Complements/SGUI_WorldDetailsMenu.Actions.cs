using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.IO.Handlers;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Complements
{
    internal sealed partial class SGUI_WorldDetailsMenu
    {
        private void ReturnButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        private void DeleteButtonAction()
        {
            SWorldSavingHandler.DeleteSavedFile(this.worldSaveFile.Metadata.Name);
            this.SGameInstance.GUIManager.CloseGUI();
        }

        private void PlayButtonAction()
        {
            this.SGameInstance.GUIManager.Reset();
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.MAIN_MENU_IDENTIFIER);
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.HUD_IDENTIFIER);

            this.SGameInstance.GameManager.StartGame();
            this.SGameInstance.World.LoadFromWorldSaveFile(this.worldSaveFile);
        }
    }
}
