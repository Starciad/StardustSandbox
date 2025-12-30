using StardustSandbox.Enums.Actors;

using System;

namespace StardustSandbox.Actors.Collision
{
    internal readonly struct ElementCollisionContext(ElementCollisionOrientation orientation, ArraySegment<ElementCollisionInfo> collisions)
    {
        internal readonly ElementCollisionOrientation Orientation => orientation;
        internal readonly ArraySegment<ElementCollisionInfo> Collisions => collisions;

        internal bool Any()
        {
            return this.Collisions.Count > 0;
        }
    }
}
