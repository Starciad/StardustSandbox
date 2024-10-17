using StardustSandbox.Game.GUISystem.Elements;

namespace StardustSandbox.Game.GameContent.GUISystem.Elements
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
