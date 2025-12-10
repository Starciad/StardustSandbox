using StardustSandbox.Elements.Utilities;

namespace StardustSandbox.Elements.Solids.Immovables.Pushers
{
    internal sealed class RightwardPusher : ImmovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            PusherUtility.PushingNeighborsRight(context, neighbors);
        }
    }
}
