using StardustSandbox.Core.Interfaces.Tools.Contexts;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Tools;
using StardustSandbox.Core.World.Slots;

namespace StardustSandbox.ContentBundle.Tools
{
    public sealed class SHeatTool(string identifier) : STool(identifier)
    {
        public override void Execute(ISToolContext context)
        {
            if (!context.World.TryGetWorldSlot(context.Position, out SWorldSlot worldSlot))
            {
                return;
            }

            SWorldSlotLayer worldSlotLayer = worldSlot.GetLayer(context.Layer);

            if (worldSlotLayer.IsEmpty)
            {
                return;
            }

            context.World.SetElementTemperature(context.Position, context.Layer, STemperatureMath.Clamp(worldSlotLayer.Temperature + 5));
        }
    }
}
