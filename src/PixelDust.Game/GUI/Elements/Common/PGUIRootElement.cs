namespace PixelDust.Game.GUI.Elements.Common
{
    public sealed class PGUIRootElement : PGUIContainerElement
    {
        public PGUIRootElement()
        {
            this.ShouldUpdate = false;
            this.IsVisible = false;
        }
    }
}
