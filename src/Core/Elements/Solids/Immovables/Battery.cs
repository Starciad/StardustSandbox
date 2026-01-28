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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Elements.Solids.Immovables
{
    internal sealed class Battery : ImmovableSolid
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            void ConductElectricity(SlotLayer neighborSlotLayer, in Point neighborPosition, in Layer targetLayer)
            {
                ElementIndex neighborElementIndex = neighborSlotLayer.ElementIndex;

                context.ReplaceElement(neighborPosition, ElementIndex.Electricity);
                context.SetStoredElement(neighborPosition, targetLayer, neighborElementIndex);
            }

            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                Slot slot = neighbors.GetSlot(i);

                SlotLayer foregroundLayer = slot.GetLayer(Layer.Foreground);
                SlotLayer backgroundLayer = slot.GetLayer(Layer.Background);

                if (!foregroundLayer.IsEmpty &&
                    foregroundLayer.ElementIndex is not ElementIndex.Electricity &&
                    foregroundLayer.Element.Characteristics.HasFlag(ElementCharacteristics.IsConductive))
                {
                    ConductElectricity(foregroundLayer, slot.Position, Layer.Foreground);
                }

                if (!backgroundLayer.IsEmpty &&
                    backgroundLayer.ElementIndex is not ElementIndex.Electricity &&
                    backgroundLayer.Element.Characteristics.HasFlag(ElementCharacteristics.IsConductive))
                {
                    ConductElectricity(backgroundLayer, slot.Position, Layer.Background);
                }
            }
        }
    }
}
