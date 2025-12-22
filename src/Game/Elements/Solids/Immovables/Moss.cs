using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
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

                switch (neighbors.GetSlotLayer(i, context.Layer).Element.Index)
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
