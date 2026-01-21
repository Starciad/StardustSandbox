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
using StardustSandbox.Core.Extensions;

using System.Collections.Generic;

namespace StardustSandbox.Core.Elements.Solids.Immovables
{
    internal sealed class Moss : ImmovableSolid
    {
        // For each direction, define the two eligible positions for spreading moss.
        private static readonly Point[][] eligibleSpreadPositions =
        [
            [new(-1, 0), new(0, -1)],  // [X] NW: W, N
            [new(-1, -1), new(1, -1)], // [X] N: NW, NE
            [new(0, -1), new(1, 0)],   // [X] NE: N, E
            [new(-1, 1), new(-1, -1)], // [X] W: SW, NW
            [new(1, -1), new(1, 1)],   // [X] E: NE, SE
            [new(0, 1), new(-1, 0)],   // [X] SW: S, W
            [new(1, 1), new(-1, 1)],   // [X] S: SE, SW
            [new(1, 0), new(0, 1)],    // [X] SE: E, S
        ];

        private static readonly HashSet<Point> eligiblePositions = [];
        private static readonly List<Point> availablePositions = [];

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            eligiblePositions.Clear();
            availablePositions.Clear();

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).ElementIndex)
                {
                    case ElementIndex.Dirt:
                    case ElementIndex.Mud:
                    case ElementIndex.Stone:
                    case ElementIndex.Wood:
                    case ElementIndex.MountingBlock:
                    case ElementIndex.RedBrick:
                    case ElementIndex.DrySponge:
                    case ElementIndex.Water:
                    case ElementIndex.Iron:
                    case ElementIndex.Glass:
                    case ElementIndex.WetBlackWool:
                    case ElementIndex.WetWhiteWool:
                    case ElementIndex.WetRedWool:
                    case ElementIndex.WetOrangeWool:
                    case ElementIndex.WetYellowWool:
                    case ElementIndex.WetGreenWool:
                    case ElementIndex.WetGrayWool:
                    case ElementIndex.WetBlueWool:
                    case ElementIndex.WetVioletWool:
                    case ElementIndex.WetBrownWool:
                    case ElementIndex.BlackPaint:
                    case ElementIndex.WhitePaint:
                    case ElementIndex.RedPaint:
                    case ElementIndex.OrangePaint:
                    case ElementIndex.YellowPaint:
                    case ElementIndex.GreenPaint:
                    case ElementIndex.BluePaint:
                    case ElementIndex.GrayPaint:
                    case ElementIndex.VioletPaint:
                    case ElementIndex.BrownPaint:
                        // For each valid neighbor, add the two eligible positions.
                        for (int j = 0; j < 2; j++)
                        {
                            _ = eligiblePositions.Add(context.Position + eligibleSpreadPositions[i][j]);
                        }

                        break;

                    default:
                        break;
                }
            }

            if (eligiblePositions.Count == 0)
            {
                return;
            }

            foreach (Point eligiblePosition in eligiblePositions)
            {
                if (context.IsEmptySlotLayer(eligiblePosition))
                {
                    availablePositions.Add(eligiblePosition);
                }
            }

            if (availablePositions.Count == 0)
            {
                return;
            }

            context.InstantiateElement(availablePositions.GetRandomItem(), context.Layer, ElementIndex.Moss);
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 100.0f)
            {
                context.ReplaceElement(ElementIndex.Fire);
            }
        }
    }
}

