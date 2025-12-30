using StardustSandbox.Enums.Actors;

namespace StardustSandbox.Actors.Collision
{
    internal readonly struct ElementCollisionContext(ElementCollisionOrientation orientation)
    {
        internal readonly ElementCollisionOrientation Orientation => orientation;
    }
}
