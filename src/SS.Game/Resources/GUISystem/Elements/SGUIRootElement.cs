using StardustSandbox.Game.Interfaces;

namespace StardustSandbox.Game.Resources.GUISystem.Elements
{
    public sealed class SGUIRootElement : SGUIContainerElement
    {
        public SGUIRootElement(ISGame gameInstance) : base(gameInstance)
        {
            this.ShouldUpdate = false;
            this.IsVisible = false;
        }
    }
}
