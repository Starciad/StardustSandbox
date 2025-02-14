using Microsoft.Xna.Framework;

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.IO.Handlers;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options
{
    internal sealed partial class SGUI_OptionsMenu
    {
        // ================================== //
        // Button Methods

        private void SaveButtonAction()
        {
            SaveSettings();
            ApplySettings();

            if (!this.restartMessageAppeared)
            {
                this.guiMessage.SetContent(SLocalization_Messages.Settings_RestartRequired);
                this.SGameInstance.GUIManager.OpenGUI(this.guiMessage.Identifier);

                this.restartMessageAppeared = true;
            }
        }

        #region SAVE SETTINGS
        private void SaveSettings()
        {
            SaveGeneralSettings();
            SaveGameplaySettings();
            SaveVolumeSettings();
            SaveVideoSettings();
            SaveGraphicsSettings();
            SaveCursorSettings();
        }

        private void SaveGeneralSettings()
        {
            SSection generalSection = this.root.Sections["general"];
            SGameCulture gameCulture = SLocalizationConstants.GetGameCulture(Convert.ToString(generalSection.Options["language"].GetValue()));

            this.generalSettings.Language = gameCulture.Language;
            this.generalSettings.Region = gameCulture.Region;

            SSettingsHandler.SaveSettings(this.generalSettings);
        }

        private void SaveGameplaySettings()
        {
            SSection gameplaySection = this.root.Sections["gameplay"];

            this.gameplaySettings.PreviewAreaColor = (Color)gameplaySection.Options["preview_area_color"].GetValue();
            this.gameplaySettings.PreviewAreaColorA = Convert.ToByte(gameplaySection.Options["preview_area_opacity"].GetValue());

            SSettingsHandler.SaveSettings(this.gameplaySettings);
        }

        private void SaveVolumeSettings()
        {
            SSection volumeSection = this.root.Sections["volume"];

            this.volumeSettings.MasterVolume = Convert.ToSingle(volumeSection.Options["master_volume"].GetValue()) / 100;
            this.volumeSettings.MusicVolume = Convert.ToSingle(volumeSection.Options["music_volume"].GetValue()) / 100;
            this.volumeSettings.SFXVolume = Convert.ToSingle(volumeSection.Options["sfx_volume"].GetValue()) / 100;

            SSettingsHandler.SaveSettings(this.volumeSettings);
        }

        private void SaveVideoSettings()
        {
            SSection videoSection = this.root.Sections["video"];

            this.videoSettings.Resolution = (SSize2)videoSection.Options["resolution"].GetValue();
            this.videoSettings.FullScreen = Convert.ToBoolean(videoSection.Options["fullscreen"].GetValue());
            this.videoSettings.VSync = Convert.ToBoolean(videoSection.Options["vsync"].GetValue());
            this.videoSettings.Borderless = Convert.ToBoolean(videoSection.Options["borderless"].GetValue());

            SSettingsHandler.SaveSettings(this.videoSettings);
        }

        private void SaveGraphicsSettings()
        {
            // SSection graphicsSettings = this.root.Sections["graphics"];

            SSettingsHandler.SaveSettings(this.graphicsSettings);
        }

        private void SaveCursorSettings()
        {
            SSection cursorSettings = this.root.Sections["cursor"];

            this.cursorSettings.Color = (Color)cursorSettings.Options["color"].GetValue();
            this.cursorSettings.BackgroundColor = (Color)cursorSettings.Options["background_color"].GetValue();
            this.cursorSettings.Alpha = Convert.ToByte(cursorSettings.Options["opacity"].GetValue());
            this.cursorSettings.Scale = Convert.ToSingle(cursorSettings.Options["scale"].GetValue());

            SSettingsHandler.SaveSettings(this.cursorSettings);
        }
        #endregion

        #region Apply Settings
        private void ApplySettings()
        {
            ApplyGeneralSettings();
            ApplyGameplaySettings();
            ApplyVolumeSettings();
            ApplyVideoSettings();
            ApplyGraphicsSettings();
            ApplyCursorSettings();
        }

        private void ApplyGeneralSettings()
        {

        }

        private void ApplyGameplaySettings()
        {

        }

        private void ApplyVolumeSettings()
        {
            SSongEngine.Volume = this.volumeSettings.MusicVolume * this.volumeSettings.MasterVolume;
            SSoundEngine.Volume = this.volumeSettings.SFXVolume * this.volumeSettings.MasterVolume;
        }

        private void ApplyVideoSettings()
        {
            this.SGameInstance.GraphicsManager.ApplySettings();
        }

        private void ApplyGraphicsSettings()
        {

        }

        private void ApplyCursorSettings()
        {
            this.SGameInstance.CursorManager.ApplySettings();
        }

        #endregion

        private void ReturnButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
