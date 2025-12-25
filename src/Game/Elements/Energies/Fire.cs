using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

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
                ElementIndex targetElementIndex = context.GetElement(targetPosition, context.Layer);
                Element targetElement = ElementDatabase.GetElement(targetElementIndex);

                if (targetElementIndex is not ElementIndex.None && (targetElement.Category is ElementCategory.MovableSolid or ElementCategory.Liquid or ElementCategory.Gas))
                {
                    context.SwappingElements(targetPosition);
                }
            }
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                if (!neighbors.GetSlotLayer(i, Layer.Foreground).IsEmpty)
                {
                    IgniteElement(context, neighbors.GetSlot(i), neighbors.GetSlotLayer(i, Layer.Foreground), Layer.Foreground);
                }

                if (!neighbors.GetSlotLayer(i, Layer.Background).IsEmpty)
                {
                    IgniteElement(context, neighbors.GetSlot(i), neighbors.GetSlotLayer(i, Layer.Background), Layer.Background);
                }
            }
        }

        private static void IgniteElement(ElementContext context, Slot slot, SlotLayer slotLayer, Layer layer)
        {
            // Increase neighboring temperature by fire's heat value
            context.SetElementTemperature(slotLayer.Temperature + ElementConstants.FIRE_HEAT_VALUE);

            // Check if the element is flammable
            if (slotLayer.Element.Characteristics.HasFlag(ElementCharacteristics.IsFlammable))
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
                if (SSRandom.Chance(combustionChance, 100.0f + slotLayer.Element.DefaultFlammabilityResistance))
                {
                    context.ReplaceElement(slot.Position, layer, ElementIndex.Fire);
                }
            }
        }
    }
}
