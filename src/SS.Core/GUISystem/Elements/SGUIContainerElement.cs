using StardustSandbox.Game.GUISystem.Elements;
using StardustSandbox.Game.Interfaces;

namespace StardustSandbox.Game.Resources.GUISystem.Elements
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
