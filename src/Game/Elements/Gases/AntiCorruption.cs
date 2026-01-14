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
using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class AntiCorruption : Gas
    {
        private static readonly List<Slot> cachedCorruptionNeighborSlots = [];

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            cachedCorruptionNeighborSlots.Clear();

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                Slot slot = neighbors.GetSlot(i);

                if (slot.GetLayer(context.Layer).Element.Characteristics.HasFlag(ElementCharacteristics.IsCorruption))
                {
                    cachedCorruptionNeighborSlots.Add(slot);
                }
            }

            if (cachedCorruptionNeighborSlots.Count > 0)
            {
                Slot corruptionNeighborSlot = cachedCorruptionNeighborSlots.GetRandomItem();
                SlotLayer neighborCorruptionSlotLayer = corruptionNeighborSlot.GetLayer(context.Layer);

                ElementIndex currentStoredElement = context.GetStoredElement();
                ElementIndex neighborStoredElement = neighborCorruptionSlotLayer.StoredElementIndex;

                Point oldPosition = context.Slot.Position;
                Point newPosition = corruptionNeighborSlot.Position;

                context.SwappingElements(oldPosition, newPosition, context.Layer);

                if (currentStoredElement is ElementIndex.None)
                {
                    context.ReplaceElement(oldPosition, ElementIndex.AntiCorruption);
                }
                else
                {
                    context.ReplaceElement(oldPosition, currentStoredElement);
                }

                context.SetStoredElement(newPosition, neighborStoredElement);
            }
            else if (context.GetStoredElement() is not ElementIndex.None)
            {
                context.ReplaceElement(context.GetStoredElement());
            }
            else if (Random.Chance(15))
            {
                context.DestroyElement();
            }
            else
            {
                context.NotifyChunk();
            }
        }
    }
}

