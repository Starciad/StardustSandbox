using Microsoft.Xna.Framework;

namespace StardustSandbox.Colors
{
    internal readonly struct GradientColor(Color start, Color end)
    {
        public readonly Color Start => start;
        public readonly Color End => end;
    }
}
