using Microsoft.Xna.Framework;

using StardustSandbox.Enums.UISystem;

namespace StardustSandbox.UISystem.Elements.TextSystem
{
    internal readonly struct BorderDirectionOffset(LabelBorderDirection direction, Vector2 offset)
    {
        internal readonly LabelBorderDirection Direction => direction;
        internal readonly Vector2 Offset => offset;
    }
}
