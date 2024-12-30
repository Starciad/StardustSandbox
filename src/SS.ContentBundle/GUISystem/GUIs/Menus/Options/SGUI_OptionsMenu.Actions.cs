﻿using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.IO.Handlers;
using StardustSandbox.Core.Localization;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_OptionsMenu
    {
        // ================================== //
        // Button Methods

        private void SaveButtonAction()
        {
            SaveVideoSettings();
            SaveLanguageSettings();
        }

        private void ReturnButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        // ================================== //
        // Saving Methods

        private void SaveVideoSettings()
        {
            this.videoSettings.Resolution = SScreenConstants.RESOLUTIONS[this.videoSectionOptionSelectors[(byte)SVideoSetting.Resolution].SelectedValueIndex];
            this.videoSettings.FullScreen = this.videoSectionOptionSelectors[(byte)SVideoSetting.Fullscreen].SelectedValue.Equals(SLocalization_Statements.True);
            this.videoSettings.VSync = this.videoSectionOptionSelectors[(byte)SVideoSetting.VSync].SelectedValue.Equals(SLocalization_Statements.True);
            this.videoSettings.Borderless = this.videoSectionOptionSelectors[(byte)SVideoSetting.Borderless].SelectedValue.Equals(SLocalization_Statements.True);

            SSettingsHandler.SaveSettings(this.videoSettings);
        }

        private void SaveLanguageSettings()
        {
            SGameCulture gameCulture = SLocalizationConstants.AVAILABLE_GAME_CULTURES[this.selectedLanguageIndex];

            this.languageSettings.Language = gameCulture.Language;
            this.languageSettings.Region = gameCulture.Region;

            SSettingsHandler.SaveSettings(this.languageSettings);
        }
    }
}
