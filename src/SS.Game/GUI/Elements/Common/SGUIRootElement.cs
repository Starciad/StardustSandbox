namespace StardustSandbox.Game.GUI.Elements.Common
{
    public sealed class SGUIRootElement : SGUIContainerElement
    {
        public SGUIRootElement()
        {
            this.ShouldUpdate = false;
            this.IsVisible = false;
        }
    }
}
