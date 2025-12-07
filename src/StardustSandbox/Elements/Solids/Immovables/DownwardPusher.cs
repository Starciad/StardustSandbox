using StardustSandbox.Elements.Utilities;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class DownwardPusher : ImmovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            PusherUtility.PushingNeighborsDown(context, neighbors);
        }
    }
}
