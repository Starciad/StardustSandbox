using StardustSandbox.Elements.Utilities;

namespace StardustSandbox.Elements.Solids.Immovables.Pushers
{
    internal sealed class LeftwardPusher : ImmovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            PusherUtility.PushingNeighborsLeft(context, neighbors);
        }
    }
}
