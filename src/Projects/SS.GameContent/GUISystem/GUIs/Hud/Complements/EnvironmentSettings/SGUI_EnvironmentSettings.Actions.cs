using System;

namespace StardustSandbox.GameContent.GUISystem.GUIs.Hud.Complements.EnvironmentSettings
{
    internal sealed partial class SGUI_EnvironmentSettings
    {
        // Menu
        private void ExitButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        // Time
        private void SetTimeFreezeState(bool value)
        {
            this.world.Time.IsFrozen = value;
        }

        private void SetTimeButtonAction(TimeSpan value)
        {
            this.world.Time.SetTime(value);
        }
    }
}
