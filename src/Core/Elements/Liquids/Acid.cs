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
using StardustSandbox.Core.Randomness;

namespace StardustSandbox.Core.Elements.Liquids
{
    internal sealed class Acid : Liquid
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).ElementIndex)
                {
                    case ElementIndex.Acid:
                    case ElementIndex.Wall:
                    case ElementIndex.Clone:
                    case ElementIndex.Void:
                    case ElementIndex.DownwardPusher:
                    case ElementIndex.UpwardPusher:
                    case ElementIndex.LeftwardPusher:
                    case ElementIndex.RightwardPusher:
                        continue;

                    default:
                        break;
                }

                if (Random.GetBool())
                {
                    context.DestroyElement(neighbors.GetNeighborPosition(i), context.Layer);
                    context.DestroyElement();
                    GameStatistics.IncrementWorldCorrodedElements();
                }
            }
        }
    }
}
