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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Elements.Gases
{
    internal sealed class AntiCorruption : Gas
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            if (context.HasStoredElement())
            {
                if (context.HasElementState(ElementStates.IsDissipating))
                {
                    context.ReplaceElement(context.GetStoredElement());
                    return;
                }
                else
                {
                    context.SetElementState(ElementStates.IsDissipating);
                }
            }

            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                Slot slot = neighbors.GetSlot(i);
                SlotLayer layer = slot.GetLayer(context.Layer);

                if (!layer.IsEmpty &&
                    layer.ElementIndex is not ElementIndex.AntiCorruption &&
                    layer.Element.HasCharacteristic(ElementCharacteristics.IsCorruption))
                {
                    ElementIndex originalElementIndex = layer.StoredElementIndex;

                    context.ReplaceElement(slot.Position, ElementIndex.AntiCorruption);
                    context.SetStoredElement(slot.Position, context.Layer, originalElementIndex);
                }
            }
        }

        protected override void OnStep(ElementContext context)
        {
            if (!context.HasStoredElement())
            {
                base.OnStep(context);
            }
        }
    }
}
