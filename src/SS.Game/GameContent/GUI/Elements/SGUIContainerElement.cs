using StardustSandbox.Game.GUI.Elements;

namespace StardustSandbox.Game.GameContent.GUI.Elements
{
    public class SGUIContainerElement : SGUIElement
    {
        public SGUIContainerElement(SGame gameInstance) : base(gameInstance)
        {
            this.ShouldUpdate = false;
            this.IsVisible = false;
        }
    }
}
