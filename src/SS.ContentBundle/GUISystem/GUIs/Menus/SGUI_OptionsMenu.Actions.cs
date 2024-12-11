using StardustSandbox.ContentBundle.Localization;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers.IO;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_OptionsMenu
    {
        public void SaveButton()
        {
            SaveVideoSettings();
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

        private void SaveLanguageSettings()
        {
            SGameCulture gameCulture = SLocalizationConstants.AVAILABLE_GAME_CULTURES[this.selectedLanguageIndex];

            this.languageSettings.Language = gameCulture.Language;
            this.languageSettings.Region = gameCulture.Region;

            SSettingsManager.SaveSettings(this.languageSettings);
        }

        public void ReturnButton()
        {
            this.SGameInstance.GUIManager.CloseGUI(this.Identifier);
            this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.MAIN_MENU_IDENTIFIER);
        }
    }
}
