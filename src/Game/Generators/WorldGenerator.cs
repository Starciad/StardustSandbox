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

using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.Generators;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.Managers;
using StardustSandbox.Mathematics;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Generators
{
    internal static class WorldGenerator
    {
        private static readonly ElementIndex[] undergroundElements =
        [
            ElementIndex.Stone,
            ElementIndex.Iron
        ];

        internal static void Start(ActorManager actorManager, World world, in WorldGenerationPreset preset)
        {
            int width = world.Information.Size.X;
            int height = world.Information.Size.Y;

            GameHandler.Reset(actorManager, world);

            Range amplitudeRange;
            WorldGenerationFlags flags;

            switch (preset)
            {
                case WorldGenerationPreset.Plain:
                    amplitudeRange = new(
                        (int)PercentageMath.PercentageOfValue(height, 30.0f),
                        (int)PercentageMath.PercentageOfValue(height, 60.0f)
                    );
                    flags = WorldGenerationFlags.HasTrees;
                    break;

                default:
                    amplitudeRange = new(
                        (int)PercentageMath.PercentageOfValue(height, 30.0f),
                        (int)PercentageMath.PercentageOfValue(height, 60.0f)
                    );
                    flags = WorldGenerationFlags.None;
                    break;
            }

            // Create a coherent height map first (single pass) and share it between generators.
            int[] heightMap = GenerateHeightMap(in width, in height, in amplitudeRange);

            GenerateTerrain(world, heightMap);
            GenerateOceans(world, heightMap);

            if (flags.HasFlag(WorldGenerationFlags.HasTrees))
            {
                GenerateTrees(world, heightMap);
            }
        }

        private static int[] GenerateHeightMap(in int width, in int height, in Range amplitudeRange)
        {
            int[] heights = new int[width];
            int baseline = (int)PercentageMath.PercentageOfValue(height, 60.0f);
            heights[0] = baseline;

            for (int x = 1; x < width; x++)
            {
                int delta = Core.Random.Range(-2, 2);
                int candidate = heights[x - 1] + delta;

                if (candidate < amplitudeRange.Start.Value)
                {
                    candidate = amplitudeRange.Start.Value;
                }
                else if (candidate > amplitudeRange.End.Value)
                {
                    candidate = amplitudeRange.End.Value;
                }

                heights[x] = candidate;
            }

            // Apply a few smoothing passes (moving average) to remove jitter
            for (int pass = 0; pass < 3; pass++)
            {
                int[] temp = new int[width];

                for (int x = 0; x < width; x++)
                {
                    int left = (x - 1 >= 0) ? heights[x - 1] : heights[x];
                    int right = (x + 1 < width) ? heights[x + 1] : heights[x];

                    // weighted average: center*2 + left + right
                    int smoothed = ((2 * heights[x]) + left + right) / 4;

                    if (smoothed < amplitudeRange.Start.Value)
                    {
                        smoothed = amplitudeRange.Start.Value;
                    }
                    else if (smoothed > amplitudeRange.End.Value)
                    {
                        smoothed = amplitudeRange.End.Value;
                    }

                    temp[x] = smoothed;
                }

                // copy back
                for (int x = 0; x < width; x++)
                {
                    heights[x] = temp[x];
                }
            }

            return heights;
        }

        private static void GenerateTerrain(World world, int[] heightMap)
        {
            int width = heightMap.Length;
            int height = world.Information.Size.Y;

            for (int x = 0; x < width; x++)
            {
                int startY = heightMap[x];

                // Randomize layer thickness per column but keep it bounded
                int grassLayer = Core.Random.Range(2, 4);
                int dirtLayer = grassLayer + Core.Random.Range(4, 8);

                int depthLevelLimit = height - startY;
                int deepThreshold = (int)PercentageMath.PercentageOfValue(depthLevelLimit, 80.0f);

                // Fill column from startY to bottom with appropriate materials
                for (int y = startY; y < height; y++)
                {
                    int relativeDepth = y - startY;
                    ElementIndex chosen = relativeDepth <= grassLayer
                        ? ElementIndex.Grass
                        : relativeDepth <= dirtLayer
                            ? ElementIndex.Dirt
                            : relativeDepth <= deepThreshold ? undergroundElements.GetRandomItem() : ElementIndex.Obsidian;

                    world.InstantiateElement(new(x, y), Layer.Foreground, chosen);
                    world.InstantiateElement(new(x, y), Layer.Background, chosen);
                }
            }
        }

        private static void GenerateOceans(World world, int[] heightMap)
        {
            int width = heightMap.Length;
            int height = world.Information.Size.Y;

            int leftOceanPointX = (int)PercentageMath.PercentageOfValue(width, 5.0f);
            int rightOceanPointX = (int)PercentageMath.PercentageOfValue(width, 95.0f);

            int oceansRadius = (int)PercentageMath.PercentageOfValue(height, 20.0f);
            int sandRadius = oceansRadius + (int)PercentageMath.PercentageOfValue(height, 10.0f);

            int leftStartTerrainIndex = heightMap[leftOceanPointX];
            int rightStartTerrainIndex = heightMap[rightOceanPointX];

            Point leftCenter = new(leftOceanPointX, leftStartTerrainIndex);
            Point rightCenter = new(rightOceanPointX, rightStartTerrainIndex);

            // Generate sand band first (keeps shoreline consistent)
            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(leftCenter, sandRadius))
            {
                if (world.IsWithinBounds(in point))
                {
                    world.ReplaceElement(point, Layer.Foreground, ElementIndex.Sand);
                    world.ReplaceElement(point, Layer.Background, ElementIndex.Sand);
                }
            }

            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(rightCenter, sandRadius))
            {
                if (world.IsWithinBounds(in point))
                {
                    world.ReplaceElement(point, Layer.Foreground, ElementIndex.Sand);
                    world.ReplaceElement(point, Layer.Background, ElementIndex.Sand);
                }
            }

            // Generate water inside the sand band
            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(leftCenter, oceansRadius))
            {
                if (world.IsWithinBounds(in point))
                {
                    world.ReplaceElement(point, Layer.Foreground, ElementIndex.Water);
                    world.ReplaceElement(point, Layer.Background, ElementIndex.Water);
                }
            }

            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(rightCenter, oceansRadius))
            {
                if (world.IsWithinBounds(in point))
                {
                    world.ReplaceElement(point, Layer.Foreground, ElementIndex.Water);
                    world.ReplaceElement(point, Layer.Background, ElementIndex.Water);
                }
            }
        }

        private static void GenerateTrees(World world, int[] heightMap)
        {
            int width = heightMap.Length;
            int height = world.Information.Size.Y;

            for (int x = 0; x < width; x++)
            {
                int surfaceY = heightMap[x];

                // Ensure we are in world bounds
                if (surfaceY <= 0 || surfaceY >= height)
                {
                    continue;
                }

                // Confirm top element is grass
                if (Core.Random.Chance(25) && world.TryGetElement(new(x, surfaceY), Layer.Foreground, out ElementIndex index) && index is ElementIndex.Grass)
                {
                    Point origin = new(x, surfaceY - 1);
                    int trunkHeight = Core.Random.Range(6, 12);
                    int trunkThickness = 1;
                    int crownRadius = Core.Random.Range(2, 4);

                    TreeGenerator.Start(world, origin, trunkHeight, trunkThickness, crownRadius);
                }
            }
        }
    }
}

