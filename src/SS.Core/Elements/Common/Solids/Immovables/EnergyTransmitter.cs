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
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.WorldSystem.Slots;

namespace StardustSandbox.Core.Elements.Common.Solids.Immovables
{
    internal sealed class EnergyTransmitter : ImmovableSolid
    {
        internal EnergyTransmitter(ElementIndex index, ElementCategory category, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents) : base(index, category, renderingType, textureOriginOffset, referenceColor, gameEvents)
        {
            this.InitialTemperature = 25.0f;
            this.BaseFlammabilityResistance = 30.0f;
            this.BaseDensity = 1.3f;
            this.BaseExplosionResistance = 1.2f;

            this.HasNeighborInteractions = true;
            this.HasTemperature = true;
            this.IsCorruptible = true;
            this.IsPushable = true;
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            bool electrifiedNeighborFound = false;

            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                if (ElementNeighbors.IsDiagonalNeighbor(i) || !neighbors.HasNeighbor(i))
                {
                    continue;
                }

                SlotLayer layer = neighbors.GetSlotLayer(i, context.CurrentLayer);

                if (!layer.IsEmpty && layer.Element.IsElectrified)
                {
                    electrifiedNeighborFound = true;
                    break;
                }
            }

            Layer oppositeLayer = context.CurrentLayer.GetOppositeLayer();

            if (electrifiedNeighborFound && !context.IsEmptySlotLayer(context.CurrentPosition, oppositeLayer))
            {
                ElectricityUtility.Electrify(context, context.CurrentPosition, oppositeLayer);
            }
        }
    }
}
