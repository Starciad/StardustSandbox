using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Mathematics;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Tools.Environment
{
    internal sealed class FreezeTool : Tool
    {
        internal override void Execute(ToolContext context)
        {
            if (!context.World.TryGetSlot(context.Position, out Slot slot))
            {
                return;
            }

            SlotLayer slotLayer = slot.GetLayer(context.Layer);

            if (slotLayer.HasState(ElementStates.IsEmpty))
            {
                return;
            }

            context.World.SetElementTemperature(context.Position, context.Layer, TemperatureMath.Clamp(slotLayer.Temperature + ToolConstants.DEFAULT_FREEZE_VALUE));
        }
    }
}
