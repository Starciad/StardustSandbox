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
            this.SGameInstance.World.StartNew(size);
            ExitButtonAction();
        }
    }
}
