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
using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Randomness;

namespace StardustSandbox.Core.Elements.Energies
{
    internal sealed class Electricity : Energy
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
            else
            {
                // If electricity has no stored element, it means that it is not being conducted.
                // Then, it will fall until it finds a conductor or disappears.

                Point belowPosition = new(context.Position.X + Random.Range(-1, 1), context.Position.Y + 1);

                if (!context.TryUpdateElementPosition(belowPosition))
                {
                    context.DestroyElement();
                }
            }

            // Check if any neighbors own electrical wiring.
            // If so, you must create another element of electricity on the conductive surface.

            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                if (neighbors.IsDiagonalNeighbor(i) || !neighbors.HasNeighbor(i))
                {
                    continue;
                }

                ElectricityUtility.Electrify(context, neighbors.GetSlot(i).Position, context.Layer);
            }
        }
    }
}
