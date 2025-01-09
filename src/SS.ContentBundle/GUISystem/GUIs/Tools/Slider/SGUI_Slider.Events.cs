using StardustSandbox.Core.GUISystem;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.TextInput
{
    internal sealed partial class SGUI_Slider : SGUISystem
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
