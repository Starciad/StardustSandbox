using Microsoft.Xna.Framework;

using StardustSandbox.Constants;

namespace StardustSandbox.Mathematics
{
    internal static class WorldMath
    {
        internal static Vector2 ToWorldPosition(Vector2 globalPosition)
        {
            return new(
                (int)(globalPosition.X / WorldConstants.GRID_SIZE),
                (int)(globalPosition.Y / WorldConstants.GRID_SIZE)
            );
        }

        internal static Vector2 ToGlobalPosition(Vector2 worldPosition)
        {
            return new(
                (int)(worldPosition.X * WorldConstants.GRID_SIZE),
                (int)(worldPosition.Y * WorldConstants.GRID_SIZE)
            );
        }
    }
}
