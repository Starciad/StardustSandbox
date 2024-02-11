namespace PixelDust.Game.GUI.Elements.Common
{
    public sealed class PGUIRootElement : PGUIElement
    {
        public PGUIRootElement() : base()
        {
            this.Id = string.Empty;
        }
        public PGUIRootElement(string id) : base(id)
        {
            this.Id = id;
        }
    }
}
