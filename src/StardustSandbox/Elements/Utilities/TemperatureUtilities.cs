using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Utilities
{
    internal static class TemperatureUtilities
    {
        internal static void ModifyNeighborsTemperature(ElementContext context, IEnumerable<Slot> neighbors, TemperatureModifierMode temperatureModifierMode)
        {
            foreach (Slot neighbor in neighbors)
            {
                SlotLayer worldSlotLayer = neighbor.GetLayer(context.Layer);

                if (worldSlotLayer.Element == null || !worldSlotLayer.Element.Characteristics.HasFlag(ElementCharacteristics.HasTemperature))
                {
                    continue;
                }

                ApplyTemperature(context, neighbor.Position, worldSlotLayer, temperatureModifierMode);
            }
        }

        private static void ApplyTemperature(ElementContext context, Point targetPosition, SlotLayer worldSlotLayer, TemperatureModifierMode temperatureModifierMode)
        {
            double result = worldSlotLayer.Temperature;

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
