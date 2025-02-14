using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options.Structure;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options
{
    internal sealed partial class SGUI_OptionsMenu
    {
        protected override void OnOpened()
        {
            SyncSettingElements();
        }

        private void SyncSettingElements()
        {
            SyncGeneralSettings();
            SyncGameplaySettings();
            SyncVolumeSettings();
            SyncVideoSettings();
            SyncGraphicsSettings();
            SyncCursorSettings();
        }

        private void SyncGeneralSettings()
        {
            SSection generalSection = this.root.Sections["general"];

            generalSection.Options["language"].SetValue(this.generalSettings.GameCulture.Name);
        }

        private void SyncGameplaySettings()
        {
            SSection gameplaySection = this.root.Sections["gameplay"];

            gameplaySection.Options["preview_area_color"].SetValue(new Color(this.gameplaySettings.PreviewAreaColor, 255));
            gameplaySection.Options["preview_area_opacity"].SetValue(this.gameplaySettings.PreviewAreaColorA);
        }

        private void SyncVolumeSettings()
        {
            SSection volumeSection = this.root.Sections["volume"];

            volumeSection.Options["master_volume"].SetValue(this.volumeSettings.MasterVolume * 100);
            volumeSection.Options["music_volume"].SetValue(this.volumeSettings.MusicVolume * 100);
            volumeSection.Options["sfx_volume"].SetValue(this.volumeSettings.SFXVolume * 100);
        }

        private void SyncVideoSettings()
        {
            SSection videoSection = this.root.Sections["video"];

            videoSection.Options["resolution"].SetValue(this.videoSettings.Resolution);
            videoSection.Options["fullscreen"].SetValue(this.videoSettings.FullScreen);
            videoSection.Options["vsync"].SetValue(this.videoSettings.VSync);
            videoSection.Options["borderless"].SetValue(this.videoSettings.Borderless);
        }

        private void SyncGraphicsSettings()
        {
            // SSection graphicsSettings = this.root.Sections["graphics"];
        }

        private void SyncCursorSettings()
        {
            SSection cursorSettings = this.root.Sections["cursor"];

            cursorSettings.Options["color"].SetValue(new Color(this.cursorSettings.Color, 255));
            cursorSettings.Options["background_color"].SetValue(new Color(this.cursorSettings.BackgroundColor, 255));
            cursorSettings.Options["opacity"].SetValue(this.cursorSettings.Alpha);
            cursorSettings.Options["scale"].SetValue(this.cursorSettings.Scale);
        }
    }
}
