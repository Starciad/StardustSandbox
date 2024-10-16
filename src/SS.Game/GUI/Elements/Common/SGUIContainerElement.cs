namespace StardustSandbox.Game.GUI.Elements.Common
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
