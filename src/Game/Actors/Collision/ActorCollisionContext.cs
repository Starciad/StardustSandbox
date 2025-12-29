using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Actors;

namespace StardustSandbox.Actors.Collision
{
    internal readonly struct ActorCollisionContext(ActorCollisionDirection direction, Point collisionPoint)
    {
        internal ActorCollisionDirection Direction => direction;
        internal Point CollisionPoint => collisionPoint;
    }
}