using StardustSandbox.Core.GUISystem;

namespace StardustSandbox.Core.GUISystem.GUIs.Tools.ColorPicker
{
    internal sealed partial class SGUI_ColorPicker : SGUISystem
    {
        protected override void OnOpened()
        {
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = true;
            this.SGameInstance.GameInputController.Disable();
        }

        protected override void OnClosed()
        {
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = false;
            this.SGameInstance.GameInputController.Activate();
        }
    }
}
