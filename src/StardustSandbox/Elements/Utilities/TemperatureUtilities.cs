using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.World;

namespace StardustSandbox.Elements.Utilities
{
    internal static class TemperatureUtilities
    {
        internal static void ModifyNeighborsTemperature(in ElementContext context, in ElementNeighbors neighbors, TemperatureModifierMode temperatureModifierMode)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                SlotLayer worldSlotLayer = neighbors.GetSlotLayer(i, context.Layer);

                if (worldSlotLayer.Element == null || !worldSlotLayer.Element.Characteristics.HasFlag(ElementCharacteristics.HasTemperature))
                {
                    continue;
                }

                ApplyTemperature(context, neighbors.GetSlot(i).Position, worldSlotLayer, temperatureModifierMode);
            }
        }

        private static void ApplyTemperature(in ElementContext context, Point targetPosition, SlotLayer worldSlotLayer, TemperatureModifierMode temperatureModifierMode)
        {
            float result = worldSlotLayer.Temperature;

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
