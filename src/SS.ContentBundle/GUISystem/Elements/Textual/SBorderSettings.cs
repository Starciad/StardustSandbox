using Microsoft.Xna.Framework;

namespace StardustSandbox.ContentBundle.GUISystem.Elements.Textual
{
    public class SBorderSettings(bool enable, Color color, Vector2 offset)
    {
        public bool IsEnabled { get; set; } = enable;
        public Color Color { get; set; } = color;
        public Vector2 Offset { get; set; } = offset;
    }
}
