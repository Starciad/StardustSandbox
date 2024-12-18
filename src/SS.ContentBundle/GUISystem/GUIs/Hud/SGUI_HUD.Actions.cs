using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.Core.Constants.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud
{
    internal sealed partial class SGUI_HUD
    {
        #region LEFT PANEL
        #region Top Buttons
        private void WeatherSettingsButtonAction()
        {

        }

        private void PenSettingsButtonAction()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.HUD_PEN_SETTINGS_IDENTIFIER);
        }

        private void ScreenshotButtonAction()
        {

        }

        private void WorldSettingsButtonAction()
        {

        }
        #endregion

        #region Bottom Buttons
        private void PauseSimulationButtonAction()
        {
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = !this.SGameInstance.GameManager.GameState.IsSimulationPaused;
        }
        #endregion
        #endregion

        // ==================================================== //

        #region RIGHT PANEL
        #region Top Buttons
        private void GameMenuButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        private void SaveMenuButtonAction()
        {

        }
        #endregion

        #region Bottom Buttons
        private void EraserButtonAction()
        {

        }

        private void ReloadSimulationButtonAction()
        {

        }

        private void EraseEverythingButtonAction()
        {
            this.SGameInstance.EntityManager.RemoveAll();
            this.world.Clear();
        }
        #endregion
        #endregion
    }
}
