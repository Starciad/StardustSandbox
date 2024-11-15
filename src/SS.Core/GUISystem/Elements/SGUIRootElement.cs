using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.GUISystem.Elements
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
