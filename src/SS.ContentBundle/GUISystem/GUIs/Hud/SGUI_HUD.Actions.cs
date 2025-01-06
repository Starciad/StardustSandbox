using StardustSandbox.Core.Constants.GUISystem;
using StardustSandbox.Core.Enums.Simulation;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud
{
    internal sealed partial class SGUI_HUD
    {
        #region LEFT PANEL
        #region Top Buttons
        private void EnvironmentSettingsButtonAction()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.HUD_ENVIRONMENT_SETTINGS_IDENTIFIER);
        }

        private void PenSettingsButtonAction()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.HUD_PEN_SETTINGS_IDENTIFIER);
        }

        private void ScreenshotButtonAction()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.HUD_SCREENSHOT_SETTINGS_IDENTIFIER);
        }

        private void WorldSettingsButtonAction()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.HUD_WORLD_SETTINGS_IDENTIFIER);
        }

        private void InfoButtonAction()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.HUD_INFORMATION_IDENTIFIER);
        }
        #endregion

        #region Bottom Buttons
        private void PauseSimulationButtonAction()
        {
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = !this.SGameInstance.GameManager.GameState.IsSimulationPaused;
        }

        private void ChangeSimulationSpeedButtonAction()
        {
            switch (this.SGameInstance.World.Simulation.CurrentSpeed)
            {
                case SSimulationSpeed.Normal:
                    this.SGameInstance.GameManager.SetSimulationSpeed(SSimulationSpeed.Fast);
                    break;

                case SSimulationSpeed.Fast:
                    this.SGameInstance.GameManager.SetSimulationSpeed(SSimulationSpeed.VeryFast);
                    break;

                case SSimulationSpeed.VeryFast:
                    this.SGameInstance.GameManager.SetSimulationSpeed(SSimulationSpeed.Normal);
                    break;

                default:
                    this.SGameInstance.GameManager.SetSimulationSpeed(SSimulationSpeed.Normal);
                    break;
            }
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
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.HUD_SAVE_SETTINGS_IDENTIFIER);
        }
        #endregion

        #region Bottom Buttons
        private void ReloadSimulationButtonAction()
        {
            this.world.Reload();
        }

        private void EraseEverythingButtonAction()
        {
            this.world.Reset();
        }
        #endregion
        #endregion
    }
}
