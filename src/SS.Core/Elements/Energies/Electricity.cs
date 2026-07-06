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
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Randomness;

namespace StardustSandbox.Core.Elements.Energies
{
    internal sealed class Electricity : Energy
    {
        internal Electricity(ElementIndex index, ElementCategory category, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents) : base(index, category, renderingType, textureOriginOffset, referenceColor, gameEvents)
        {
            this.InitialTemperature = 20.0f;
            this.BaseDensity = 0.0f;
            this.BaseExplosionResistance = 0.0f;
            this.BaseDispersionRate = 8;

            this.HasNeighborInteractions = true;
            this.IsCorruptible = true;
            this.IsElectrified = true;
        }

        private static void UpdateDissipationOrFall(ElementContext context)
        {
            // If electricity has a stored element, it means that it is being conducted.
            if (context.HasStoredElement())
            {
                // If electricity is already dissipating, it will replace itself with the stored element.
                if (context.GetDissipatingState())
                {
                    context.Replace(context.GetStoredElementIndex());
                    return;
                }

                // If electricity is not dissipating, it will start to dissipate.
                context.SetDissipatingState(true);
                return;
            }

            // If electricity has no stored element, it means that it is not being conducted.
            // Then, it will fall until it finds a conductor or disappears.
            Point belowPosition = new(context.Position.X + Random.Range(-1, 1), context.Position.Y + 1);

            if (!context.TryUpdatePosition(belowPosition))
            {
                context.Destroy();
            }
        }

        private static void ElectrifyNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            // Check if any neighbors own electrical wiring.
            // If so, you must create another element of electricity on the conductive surface.
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (ElementNeighbors.IsDiagonalNeighbor(i) || !neighbors.HasNeighbor(i))
                {
                    continue;
                }

                ElectricityUtility.Electrify(context, neighbors.GetNeighborPosition(i), context.Layer);
            }
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            UpdateDissipationOrFall(context);
            ElectrifyNeighbors(context, neighbors);
        }
    }
}
