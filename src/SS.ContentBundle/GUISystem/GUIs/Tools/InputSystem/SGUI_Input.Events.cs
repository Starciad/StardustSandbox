using StardustSandbox.Core.GUISystem;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Specials
{
    internal sealed partial class SGUI_Input : SGUISystem
    {
        protected override void OnOpened()
        {
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = true;
        }

        protected override void OnClosed()
        {
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = false;
        }
    }
}
