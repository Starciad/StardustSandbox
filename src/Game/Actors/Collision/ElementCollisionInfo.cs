using Microsoft.Xna.Framework;

using StardustSandbox.Databases;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Actors.Collision
{
    internal readonly struct ElementCollisionInfo(ElementIndex elementIndex, Point elementPosition)
    {
        internal readonly Element Element => ElementDatabase.GetElement(elementIndex);
        internal readonly ElementIndex ElementIndex => elementIndex;
        internal readonly Point ElementPosition => elementPosition;
    }
}
