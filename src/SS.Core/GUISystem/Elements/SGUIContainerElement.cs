using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.Core.GUISystem.Elements
{
    public class SGUIContainerElement : SGUIElement
    {
        public SGUIContainerElement(ISGame gameInstance) : base(gameInstance)
        {
            this.ShouldUpdate = false;
            this.IsVisible = false;
        }
    }
}
