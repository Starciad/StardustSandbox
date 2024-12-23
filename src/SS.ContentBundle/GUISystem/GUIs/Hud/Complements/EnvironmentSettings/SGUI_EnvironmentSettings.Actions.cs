using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.Enums.World;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_EnvironmentSettings
    {
        // Menu
        private void ExitButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
