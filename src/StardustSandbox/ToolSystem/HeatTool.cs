using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Interfaces.Tools;
using StardustSandbox.Mathematics;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.ToolSystem
{
    internal sealed class HeatTool : Tool
    {
        internal override void Execute(IToolContext context)
        {
            if (!context.World.TryGetSlot(context.Position, out Slot worldSlot))
            {
                return;
            }

            SlotLayer worldSlotLayer = worldSlot.GetLayer(context.Layer);

            if (worldSlotLayer.HasState(ElementStates.IsEmpty))
            {
                return;
            }

            context.World.SetElementTemperature(context.Position, context.Layer, TemperatureMath.Clamp(worldSlotLayer.Temperature + ToolConstants.DEFAULT_HEAT_VALUE));
        }
    }
}
