using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;

namespace StardustSandbox.UI.Common.Tools
{
    internal sealed class KeySelectorUI : UIBase
    {
        private readonly GameManager gameManager;

        internal KeySelectorUI(
            GameManager gameManager,
            UIIndex index
        ) : base(index)
        {
            this.gameManager = gameManager;
        }

        protected override void OnBuild(Container root)
        {

        }

        protected override void OnOpened()
        {
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
        }

        protected override void OnClosed()
        {
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
        }
    }
}
