using StardustSandbox.Constants;
using StardustSandbox.Interfaces.Tools;
using StardustSandbox.Mathematics;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.ToolSystem
{
    internal sealed class FreezeTool : Tool
    {
        internal override void Execute(IToolContext context)
        {
            if (!context.World.TryGetSlot(context.Position, out Slot worldSlot))
            {
                return;
            }

            SlotLayer worldSlotLayer = worldSlot.GetLayer(context.Layer);

            if (worldSlotLayer.IsEmpty)
            {
                return;
            }

            context.World.SetElementTemperature(context.Position, context.Layer, TemperatureMath.Clamp(worldSlotLayer.Temperature + ToolConstants.DEFAULT_FREEZE_VALUE));
        }
    }
}
