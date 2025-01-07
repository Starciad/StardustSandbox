using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_WorldSettings
    {
        // Menu
        private void ExitButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        // Sizes
        private void SetWorldSizeButtonAction(SSize2 size)
        {
            this.SGameInstance.GUIManager.CloseGUI();
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = true;
            this.worldTargetSize = size;
            this.guiConfirm.Configure(this.changeWorldSizeConfirmSettings);
            this.SGameInstance.GUIManager.OpenGUI(this.guiConfirm.Identifier);
        }
    }
}
