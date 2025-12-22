using StardustSandbox.Elements.Utilities;

namespace StardustSandbox.Elements.Solids.Immovables.Pushers
{
    internal sealed class DownwardPusher : ImmovableSolid
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            PusherUtility.PushingNeighborsDown(context, neighbors);
        }
    }
}
