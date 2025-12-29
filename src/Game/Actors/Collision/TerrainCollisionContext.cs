using StardustSandbox.Enums.Actors;

namespace StardustSandbox.Actors.Collision
{
    internal readonly struct TerrainCollisionContext(TerrainCollisionOrientation orientation)
    {
        internal readonly TerrainCollisionOrientation Orientation => orientation;
    }
}
