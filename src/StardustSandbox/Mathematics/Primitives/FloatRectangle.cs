using Microsoft.Xna.Framework;

namespace StardustSandbox.Mathematics.Primitives
{
    internal readonly struct FloatRectangle(Vector2 location, Vector2 size)
    {
        public readonly Vector2 Location => location;
        public readonly Vector2 Size => size;
    }
}
