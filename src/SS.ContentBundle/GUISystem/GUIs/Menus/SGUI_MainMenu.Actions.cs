using StardustSandbox.Core.Constants.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_MainMenu
    {
        private void CreateMenuButton()
        {
            this.SGameInstance.GameManager.StartGame();
        }

        private void PlayMenuButton()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.PLAY_MENU_IDENTIFIER);
        }

        private void OptionsMenuButton()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.OPTIONS_MENU_IDENTIFIER);
        }

        private void CreditsMenuButton()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.CREDITS_MENU_IDENTIFIER);
        }

        private void QuitMenuButton()
        {
            this.SGameInstance.Quit();
        }
    }
}
