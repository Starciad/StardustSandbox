using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Directions;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class LeftwardPusher : ImmovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            PusherUtility.Push(context, neighbors, CardinalDirection.West);
        }
    }
}
