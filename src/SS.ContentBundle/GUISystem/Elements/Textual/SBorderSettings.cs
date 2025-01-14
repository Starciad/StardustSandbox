using Microsoft.Xna.Framework;

namespace StardustSandbox.ContentBundle.GUISystem.Elements.Textual
{
    internal class SBorderSettings(bool enable, Color color, Vector2 offset)
    {
        internal bool IsEnabled { get; set; } = enable;
        internal Color Color { get; set; } = color;
        internal Vector2 Offset { get; set; } = offset;
    }
}
