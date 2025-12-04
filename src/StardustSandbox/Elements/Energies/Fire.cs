using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Randomness;
using StardustSandbox.World;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Energies
{
    internal sealed class Fire : Energy
    {
        protected override void OnBeforeStep(ElementContext context)
        {
            if (SSRandom.Chance(ElementConstants.CHANCE_OF_FIRE_TO_DISAPPEAR))
            {
                context.DestroyElement();

                if (SSRandom.Chance(ElementConstants.CHANCE_FOR_FIRE_TO_LEAVE_SMOKE))
                {
                    context.InstantiateElement(ElementIndex.Smoke);
                }
            }
        }

        protected override void OnStep(ElementContext context)
        {
            Point targetPosition = new(context.Slot.Position.X + SSRandom.Range(-1, 1), context.Slot.Position.Y - 1);

            if (context.IsEmptySlot(targetPosition))
            {
                if (context.TrySetPosition(targetPosition, context.Layer))
                {
                    return;
                }
            }
            else
            {
                Element targetElement = context.GetElement(targetPosition, context.Layer);

                if (targetElement != null && (targetElement.Category == ElementCategory.MovableSolid || targetElement.Category == ElementCategory.Liquid || targetElement.Category == ElementCategory.Gas))
                {
                    context.SwappingElements(targetPosition);
                }
            }
        }

        protected override void OnNeighbors(ElementContext context, IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                if (!neighbor.ForegroundLayer.HasState(ElementStates.IsEmpty))
                {
                    IgniteElement(context, neighbor, neighbor.GetLayer(LayerType.Foreground), LayerType.Foreground);
                }

                if (!neighbor.BackgroundLayer.HasState(ElementStates.IsEmpty))
                {
                    IgniteElement(context, neighbor, neighbor.GetLayer(LayerType.Background), LayerType.Background);
                }
            }
        }

        private static void IgniteElement(ElementContext context, Slot slot, SlotLayer worldSlotLayer, LayerType layer)
        {
            // Increase neighboring temperature by fire's heat value
            context.SetElementTemperature((short)(worldSlotLayer.Temperature + ElementConstants.FIRE_HEAT_VALUE));

            // Check if the element is flammable
            if (worldSlotLayer.Element.Characteristics.HasFlag(ElementCharacteristics.IsFlammable))
            {
                // Adjust combustion chance based on the element's flammability resistance
                int combustionChance = ElementConstants.CHANCE_OF_COMBUSTION;
                bool isAbove = slot.Position.Y < context.Slot.Position.Y;

                // Increase chance of combustion if the element is directly above
                if (isAbove)
                {
                    combustionChance += 10;
                }

                // Attempt combustion based on flammabilityResistance
                if (SSRandom.Chance(combustionChance, 100.0 + worldSlotLayer.Element.DefaultFlammabilityResistance))
                {
                    context.ReplaceElement(slot.Position, layer, ElementIndex.Fire);
                }
            }
        }
    }
}
