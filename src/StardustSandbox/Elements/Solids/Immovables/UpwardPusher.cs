using StardustSandbox.Elements.Utilities;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class UpwardPusher : ImmovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            PusherUtility.PushingNeighborsUp(in context, in neighbors);
        }
    }
}
