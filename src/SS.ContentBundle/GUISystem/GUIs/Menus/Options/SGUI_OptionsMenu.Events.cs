using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options.Structure;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.IO.Files.Settings;
using StardustSandbox.Core.IO.Handlers;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options
{
    internal sealed partial class SGUI_OptionsMenu
    {
        protected override void OnOpened()
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

            generalSection.Options["language"].SetValue(this.generalSettings.GameCulture.CultureInfo.NativeName.FirstCharToUpper());
        }

        private void SyncGameplaySettings()
        {
            SSection gameplaySection = this.root.Sections["gameplay"];
        }

        private void SyncVolumeSettings()
        {

        }

        private void SyncVideoSettings()
        {

        }

        private void SyncGraphicsSettings()
        {

        }

        private void SyncCursorSettings()
        {
        
        }
    }
}
