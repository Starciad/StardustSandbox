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

using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Randomness;
using StardustSandbox.Core.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Core.Elements.Solids.Immovables
{
    internal sealed class Devourer : ImmovableSolid
    {
        private static readonly ExplosionBuilder explosionBuilder = new()
        {
            Radius = 4,
            Power = 2.5f,
            Heat = 180,

            AffectsWater = false,
            AffectsSolids = true,
            AffectsGases = true,

            ExplosionResidues =
            [
                ElementIndex.Fire,
                ElementIndex.Smoke,
            ]
        };

        private static readonly List<Slot> cachedNeighborSlots = [];

        protected override void OnDestroyed(ElementContext context)
        {
            context.InstantiateExplosion(explosionBuilder);
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            cachedNeighborSlots.Clear();

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).ElementIndex)
                {
                    case ElementIndex.Devourer:
                    case ElementIndex.Void:
                    case ElementIndex.Clone:
                    case ElementIndex.Wall:
                        continue;

                    default:
                        break;
                }

                cachedNeighborSlots.Add(neighbors.GetSlot(i));
            }

            if (cachedNeighborSlots.Count > 0)
            {
                Slot neighborSlot = cachedNeighborSlots.GetRandomItem();

                Point oldPosition = context.Slot.Position;
                Point newPosition = neighborSlot.Position;

                context.SwappingElements(oldPosition, newPosition, context.Layer);
                context.RemoveElement(oldPosition);
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

