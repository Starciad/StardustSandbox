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
            SToolbarSlot toolbarSlot = this.leftPanelBottomButtonElements[0];

            if (this.SGameInstance.GameManager.GameState.IsSimulationPaused)
            {
                this.SGameInstance.GameManager.GameState.IsSimulationPaused = false;
                toolbarSlot.Icon.Texture = this.iconTextures[(byte)SIconIndex.Pause];
            }
            else
            {
                this.SGameInstance.GameManager.GameState.IsSimulationPaused = true;
                toolbarSlot.Icon.Texture = this.iconTextures[(byte)SIconIndex.Resume];
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
