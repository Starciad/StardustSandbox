using StardustSandbox.Elements.Utilities;

namespace StardustSandbox.Elements.Solids.Immovables.Pushers
{
    internal sealed class LeftwardPusher : ImmovableSolid
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            PusherUtility.PushingNeighborsLeft(context, neighbors);
        }
    }
}
