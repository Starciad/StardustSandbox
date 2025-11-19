using Microsoft.Xna.Framework;

namespace StardustSandbox.UISystem.Elements.Textual
{
    internal sealed class BorderSettings(bool enable, Color color, Vector2 offset)
    {
        internal bool IsEnabled { get; set; } = enable;
        internal Color Color { get; set; } = color;
        internal Vector2 Offset { get; set; } = offset;
    }
}
