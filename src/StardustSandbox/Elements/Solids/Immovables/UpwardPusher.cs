using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Directions;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class UpwardPusher : ImmovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            PusherUtility.Push(in context, in neighbors, CardinalDirection.North);
        }
    }
}
