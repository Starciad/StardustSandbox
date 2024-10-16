namespace StardustSandbox.Game.GameContent.GUI.Elements
{
    public sealed class SGUIRootElement : SGUIContainerElement
    {
        public SGUIRootElement(SGame gameInstance) : base(gameInstance)
        {
            this.ShouldUpdate = false;
            this.IsVisible = false;
        }
    }
}
