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

using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Core.Elements.Utilities
{
    internal static class CorruptionUtility
    {
        private readonly struct SlotTarget(Slot slot, Layer layer)
        {
            internal readonly Slot Slot => slot;
            internal readonly Layer Layer => layer;
        }

        private static readonly List<SlotTarget> targets = [];

        internal static bool CheckIfNeighboringElementsAreCorrupted(in Layer layer, ElementNeighbors neighbors)
        {
            int count = 0;
            int corruptNeighboringElements = 0;

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, layer))
                {
                    continue;
                }

                Element element = neighbors.GetSlotLayer(i, layer).Element;

                if (element == null)
                {
                    continue;
                }

                if (element.Characteristics.HasFlag(ElementCharacteristics.IsCorruption))
                {
                    corruptNeighboringElements++;
                }

                count++;
            }

            return corruptNeighboringElements == count;
        }

        internal static void InfectNeighboringElements(this ElementContext context, ElementNeighbors neighbors)
        {
            targets.Clear();

            void ProcessLayer(Slot slot, Layer layer, Element element)
            {
                if (element.Characteristics.HasFlag(ElementCharacteristics.IsCorruptible))
                {
                    targets.Add(new(slot, layer));
                }
            }

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                if (!neighbors.GetSlot(i).IsForegroundEmpty)
                {
                    ProcessLayer(neighbors.GetSlot(i), Layer.Foreground, neighbors.GetSlotLayer(i, Layer.Foreground).Element);
                }

                if (!neighbors.GetSlot(i).IsBackgroundEmpty)
                {
                    ProcessLayer(neighbors.GetSlot(i), Layer.Background, neighbors.GetSlotLayer(i, Layer.Background).Element);
                }
            }

            if (targets.Count == 0)
            {
                return;
            }

            InfectSlotLayer(context, targets.GetRandomItem());
        }

        private static void InfectSlotLayer(ElementContext context, in SlotTarget slotTarget)
        {
            Element targetElement = slotTarget.Layer is Layer.Foreground
                ? slotTarget.Slot.Foreground.Element
                : slotTarget.Slot.Background.Element;

            switch (targetElement.Category)
            {
                case ElementCategory.MovableSolid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.MovableCorruption);
                    break;

                case ElementCategory.ImmovableSolid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.ImmovableCorruption);
                    break;

                case ElementCategory.Liquid:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.LiquidCorruption);
                    break;

                case ElementCategory.Gas:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.GasCorruption);
                    break;

                default:
                    context.ReplaceElement(slotTarget.Slot.Position, slotTarget.Layer, ElementIndex.MovableCorruption);
                    break;
            }

            context.SetStoredElement(slotTarget.Slot.Position, slotTarget.Layer, targetElement.Index);
        }
    }
}
