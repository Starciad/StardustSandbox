using Microsoft.Xna.Framework;

using PixelDust.Game.Objects;

namespace PixelDust.Game.GUI.Elements
{
    public abstract class PGUIElement : PGameObject
    {
        // Settings
        public string Id { get; set; }

        // Readonly
        public PGUIElementStyle Style => this.style;
        public Vector2 Position => this.style.GetPosition();

        private readonly PGUIElementStyle style;

        public PGUIElement()
        {
            this.style = new(this);
            this.Id = string.Empty;
        }

        public PGUIElement(string id)
        {
            this.style = new(this);
            this.Id = id;
        }
    }
}
