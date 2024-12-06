using StardustSandbox.ContentBundle.Localization;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Managers.IO;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_OptionsMenu
    {
        public void SaveButton()
        {
            SaveVideoSettings();
            SaveVolumeSettings();
            SaveCursorSettings();
            SaveLanguageSettings();
        }

        private void SaveVideoSettings()
        {
            this.videoSettings.Resolution = SScreenConstants.RESOLUTIONS[this.videoSectionOptionSelectors[(byte)SVideoSetting.Resolution].SelectedValueIndex];
            this.videoSettings.FullScreen = this.videoSectionOptionSelectors[(byte)SVideoSetting.Fullscreen].SelectedValue.Equals(SLocalization.Statements_True);
            this.videoSettings.VSync = this.videoSectionOptionSelectors[(byte)SVideoSetting.VSync].SelectedValue.Equals(SLocalization.Statements_True);
            this.videoSettings.Borderless = this.videoSectionOptionSelectors[(byte)SVideoSetting.Borderless].SelectedValue.Equals(SLocalization.Statements_True);

            SSettingsManager.SaveSettings(this.videoSettings);
        }

        private void SaveVolumeSettings()
        {
            SSettingsManager.SaveSettings(this.volumeSettings);
        }

        private void SaveCursorSettings()
        {
            SSettingsManager.SaveSettings(this.cursorSettings);
        }

        private void SaveLanguageSettings()
        {
            this.languageSettings.SetGameCulture(SLocalizationConstants.AVAILABLE_GAME_CULTURES[this.selectedLanguageIndex]);

            SSettingsManager.SaveSettings(this.languageSettings);
        }

        public void ReturnButton()
        {
            this.SGameInstance.GUIManager.CloseGUI(this.Identifier);
            this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.MAIN_MENU_IDENTIFIER);
        }
    }
}
