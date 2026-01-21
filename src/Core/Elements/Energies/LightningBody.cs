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
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Elements.Energies
{
    internal sealed class LightningBody : Energy
    {
        private static readonly ExplosionBuilder explosionBuilder = new()
        {
            Radius = 2.0f,
            Power = 5.0f,
            Heat = TemperatureConstants.MAX_CELSIUS_VALUE,

            AffectsWater = true,
            AffectsSolids = true,
            AffectsGases = true,

            ExplosionResidues =
            [
                ElementIndex.Fire,
                ElementIndex.Smoke
            ]
        };

        protected override void OnAfterStep(ElementContext context)
        {
            context.RemoveElement();
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                SlotLayer slotLayer = neighbors.GetSlotLayer(i, context.Layer);

                if (slotLayer.Element.Category is ElementCategory.Gas)
                {
                    continue;
                }

                switch (slotLayer.ElementIndex)
                {
                    case ElementIndex.LightningBody:
                    case ElementIndex.LightningHead:
                    case ElementIndex.Clone:
                    case ElementIndex.Void:
                    case ElementIndex.Wall:
                    case ElementIndex.Fire:
                        continue;

                    case ElementIndex.Water:
                    case ElementIndex.Snow:
                    case ElementIndex.Ice:
                        if (slotLayer.HasState(ElementStates.IsFalling))
                        {
                            continue;
                        }

                        break;

                    default:
                        break;
                }

                context.InstantiateExplosion(explosionBuilder);
            }
        }
    }
}

