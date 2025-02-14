using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.Elements.Contexts;
using StardustSandbox.Core.World.Slots;
using StardustSandbox.GameContent.Enums.Elements.Utilities;

using System.Collections.Generic;

namespace StardustSandbox.GameContent.Elements.Utilities
{
    internal static class STemperatureUtilities
    {
        internal static void ModifyNeighborsTemperature(ISElementContext context, IEnumerable<SWorldSlot> neighbors, STemperatureModifierMode temperatureModifierMode)
        {
            foreach (SWorldSlot neighbor in neighbors)
            {
                SWorldSlotLayer worldSlotLayer = neighbor.GetLayer(context.Layer);

                if (!worldSlotLayer.Element.EnableFlammability)
                {
                    continue;
                }

                ApplyTemperature(context, neighbor.Position, worldSlotLayer, temperatureModifierMode);
            }
        }

        private static void ApplyTemperature(ISElementContext context, Point targetPosition, SWorldSlotLayer worldSlotLayer, STemperatureModifierMode temperatureModifierMode)
        {
            short result = worldSlotLayer.Temperature;

            switch (temperatureModifierMode)
            {
                case STemperatureModifierMode.Warming:
                    result += SToolConstants.DEFAULT_HEAT_VALUE;
                    break;

                case STemperatureModifierMode.Cooling:
                    result += SToolConstants.DEFAULT_FREEZE_VALUE;
                    break;

                default:
                    break;
            }

            context.SetElementTemperature(targetPosition, context.Layer, result);
        }
    }
}
