/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;

using StardustSandbox.Core;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Elements.Energies
{
    internal sealed class Fire : Energy
    {
        protected override void OnBeforeStep(ElementContext context)
        {
            if (Random.Chance(ElementConstants.CHANCE_OF_FIRE_TO_DISAPPEAR))
            {
                context.DestroyElement();

                if (Random.Chance(ElementConstants.CHANCE_FOR_FIRE_TO_LEAVE_SMOKE))
                {
                    context.InstantiateElement(ElementIndex.Smoke);
                }
            }
        }

        protected override void OnStep(ElementContext context)
        {
            Point targetPosition = new(context.Slot.Position.X + Random.Range(-1, 1), context.Slot.Position.Y - 1);

            if (context.IsEmptySlot(targetPosition))
            {
                if (context.TrySetPosition(targetPosition, context.Layer))
                {
                    return;
                }
            }
            else
            {
                if (!context.TryGetElement(targetPosition, context.Layer, out ElementIndex index))
                {
                    return;
                }

                Element targetElement = ElementDatabase.GetElement(index);

                if (index is not ElementIndex.None && (targetElement.Category is ElementCategory.MovableSolid or ElementCategory.Liquid or ElementCategory.Gas))
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
                if (Random.Chance(combustionChance, 100.0f + slotLayer.Element.DefaultFlammabilityResistance))
                {
                    context.ReplaceElement(slot.Position, layer, ElementIndex.Fire);
                }
            }
        }
    }
}

