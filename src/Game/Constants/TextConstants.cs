using StardustSandbox.Enums.UI;
using StardustSandbox.UI.Elements.TextSystem;

namespace StardustSandbox.Constants
{
    internal static class TextConstants
    {
        internal static readonly BorderDirectionOffset[] BORDER_DIRECTION_OFFSETS =
        [
            new(LabelBorderDirection.North,     new(0.0f, -1.0f)),
            new(LabelBorderDirection.NorthEast, new(1.0f, -1.0f)),
            new(LabelBorderDirection.East,      new(1.0f, 0.0f)),
            new(LabelBorderDirection.SouthEast, new(1.0f, 1.0f)),
            new(LabelBorderDirection.South,     new(0.0f, 1.0f)),
            new(LabelBorderDirection.SouthWest, new(-1.0f, 1.0f)),
            new(LabelBorderDirection.West,      new(-1.0f, 0.0f)),
            new(LabelBorderDirection.NorthWest, new(-1.0f, -1.0f))
        ];
    }
}
