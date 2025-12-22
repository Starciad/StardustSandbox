using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Elements.Utilities
{
    internal static class TemperatureUtilities
    {
        internal static void ModifyNeighborsTemperature(ElementContext context, ElementNeighbors neighbors, in TemperatureModifierMode temperatureModifierMode)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer) ||
                    !neighbors.GetSlotLayer(i, context.Layer).Element.Characteristics.HasFlag(ElementCharacteristics.HasTemperature))
                {
                    continue;
                }

                ApplyTemperature(context, neighbors.GetSlot(i).Position, neighbors.GetSlotLayer(i, context.Layer), temperatureModifierMode);
            }
        }

        private static void ApplyTemperature(ElementContext context, in Point targetPosition, SlotLayer slotLayer, in TemperatureModifierMode temperatureModifierMode)
        {
            float result = slotLayer.Temperature;

            switch (temperatureModifierMode)
            {
                case TemperatureModifierMode.Warming:
                    result += ToolConstants.DEFAULT_HEAT_VALUE;
                    break;

                case TemperatureModifierMode.Cooling:
                    result += ToolConstants.DEFAULT_FREEZE_VALUE;
                    break;

                default:
                    break;
            }

            context.SetElementTemperature(targetPosition, context.Layer, result);
        }
    }
}
