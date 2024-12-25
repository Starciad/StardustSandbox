using StardustSandbox.Core.Constants.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_MainMenu
    {
        private void CreateMenuButtonAction()
        {
            this.SGameInstance.GameManager.StartGame();
        }

        private void PlayMenuButtonAction()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.PLAY_MENU_IDENTIFIER);
        }

        private void OptionsMenuButtonAction()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.OPTIONS_MENU_IDENTIFIER);
        }

        private void CreditsMenuButtonAction()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.CREDITS_MENU_IDENTIFIER);
        }

        private void QuitMenuButtonAction()
        {
            this.SGameInstance.Quit();
        }
    }
}
