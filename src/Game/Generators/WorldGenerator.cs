using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.Generators;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.Managers;
using StardustSandbox.Mathematics;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Generators
{
    internal static class WorldGenerator
    {
        // small set of underground elements; readonly to avoid accidental modification
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
                    amplitudeRange = new Range(
                        (int)PercentageMath.PercentageOfValue(height, 30.0f),
                        (int)PercentageMath.PercentageOfValue(height, 60.0f)
                    );
                    flags = WorldGenerationFlags.HasTrees;
                    break;

                default:
                    amplitudeRange = new Range(
                        (int)PercentageMath.PercentageOfValue(height, 30.0f),
                        (int)PercentageMath.PercentageOfValue(height, 60.0f)
                    );
                    flags = WorldGenerationFlags.None;
                    break;
            }

            // Create a coherent height map first (single pass) and share it between generators.
            int[] heightMap = GenerateHeightMap(in width, in height, in amplitudeRange);

            GenerateTerrain(world, heightMap, in amplitudeRange, in flags);
            GenerateOceans(world, heightMap);
            if (flags.HasFlag(WorldGenerationFlags.HasTrees))
            {
                GenerateTrees(world, heightMap);
            }
        }

        // Compute the first occupied Y for a column using the precomputed height map.
        private static int GetTerrainStartIndexFromHeightMap(in int[] heightMap, in int x)
        {
            return heightMap[x];
        }

        // Safe bounds check helper
        private static bool IsPointInsideWorld(in Point point, World world)
        {
            return point.X >= 0 && point.Y >= 0 && point.X < world.Information.Size.X && point.Y < world.Information.Size.Y;
        }

        // Create a smooth height map using a random walk + smoothing passes.
        // Uses only integer math and limited allocations.
        private static int[] GenerateHeightMap(in int width, in int height, in Range amplitudeRange)
        {
            int[] heights = new int[width];

            // Start around 60% of height as baseline (same as original)
            int baseline = (int)PercentageMath.PercentageOfValue(height, 60.0f);

            // Random walk to generate rough terrain
            heights[0] = baseline;
            for (int x = 1; x < width; x++)
            {
                int delta = SSRandom.Range(-2, 3); // small step to keep slopes gentle
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
            const int smoothingPasses = 3;
            for (int pass = 0; pass < smoothingPasses; pass++)
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

        // Generate column-based terrain using the precalculated height map.
        private static void GenerateTerrain(World world, int[] heightMap, in Range amplitudeRange, in WorldGenerationFlags flags)
        {
            int width = heightMap.Length;
            int height = world.Information.Size.Y;

            for (int x = 0; x < width; x++)
            {
                int startY = heightMap[x];

                // Randomize layer thickness per column but keep it bounded
                int grassLayer = SSRandom.Range(2, 4);
                int dirtLayer = grassLayer + SSRandom.Range(4, 8);

                int depthLevelLimit = height - startY;
                int deepThreshold = (int)PercentageMath.PercentageOfValue(depthLevelLimit, 80.0f);

                // Fill column from startY to bottom with appropriate materials
                for (int y = startY; y < height; y++)
                {
                    int relativeDepth = y - startY;
                    ElementIndex chosen;

                    if (relativeDepth <= grassLayer)
                    {
                        chosen = ElementIndex.Grass;
                    }
                    else if (relativeDepth <= dirtLayer)
                    {
                        chosen = ElementIndex.Dirt;
                    }
                    else if (relativeDepth <= deepThreshold)
                    {
                        // pick one underground element for both foreground and background
                        chosen = undergroundElements.GetRandomItem();
                    }
                    else
                    {
                        chosen = ElementIndex.Obsidian;
                    }

                    // Instantiate once and set both layers to the same element to keep consistency.
                    world.InstantiateElement(new Point(x, y), Layer.Foreground, chosen);
                    world.InstantiateElement(new Point(x, y), Layer.Background, chosen);
                }
            }
        }

        // Place oceans using the height map to find surface positions.
        private static void GenerateOceans(World world, int[] heightMap)
        {
            int width = heightMap.Length;
            int height = world.Information.Size.Y;

            int leftOceanPointX = (int)PercentageMath.PercentageOfValue(width, 5.0f);
            int rightOceanPointX = (int)PercentageMath.PercentageOfValue(width, 95.0f);

            int oceansRadius = (int)PercentageMath.PercentageOfValue(height, 20.0f);
            int sandRadius = oceansRadius + (int)PercentageMath.PercentageOfValue(height, 10.0f);

            int leftStartTerrainIndex = GetTerrainStartIndexFromHeightMap(in heightMap, in leftOceanPointX);
            int rightStartTerrainIndex = GetTerrainStartIndexFromHeightMap(in heightMap, in rightOceanPointX);

            Point leftCenter = new(leftOceanPointX, leftStartTerrainIndex);
            Point rightCenter = new(rightOceanPointX, rightStartTerrainIndex);

            // Generate sand band first (keeps shoreline consistent)
            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(leftCenter, sandRadius))
            {
                if (IsPointInsideWorld(in point, world))
                {
                    world.ReplaceElement(point, Layer.Foreground, ElementIndex.Sand);
                    world.ReplaceElement(point, Layer.Background, ElementIndex.Sand);
                }
            }

            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(rightCenter, sandRadius))
            {
                if (IsPointInsideWorld(in point, world))
                {
                    world.ReplaceElement(point, Layer.Foreground, ElementIndex.Sand);
                    world.ReplaceElement(point, Layer.Background, ElementIndex.Sand);
                }
            }

            // Generate water inside the sand band
            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(leftCenter, oceansRadius))
            {
                if (IsPointInsideWorld(in point, world))
                {
                    world.ReplaceElement(point, Layer.Foreground, ElementIndex.Water);
                    world.ReplaceElement(point, Layer.Background, ElementIndex.Water);
                }
            }

            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(rightCenter, oceansRadius))
            {
                if (IsPointInsideWorld(in point, world))
                {
                    world.ReplaceElement(point, Layer.Foreground, ElementIndex.Water);
                    world.ReplaceElement(point, Layer.Background, ElementIndex.Water);
                }
            }
        }

        // Trees: sample columns and place trees on grass with a probabilistic filter.
        private static void GenerateTrees(World world, int[] heightMap)
        {
            int width = heightMap.Length;
            int height = world.Information.Size.Y;

            // To avoid checking every column, step by a small stride but keep randomness
            const int stride = 1; // keep stride 1 to maintain similar density to original; increase for fewer candidates

            for (int x = 0; x < width; x += stride)
            {
                int surfaceY = GetTerrainStartIndexFromHeightMap(in heightMap, in x);

                // Ensure we are in world bounds
                if (surfaceY <= 0 || surfaceY >= height)
                {
                    continue;
                }

                // Confirm top element is grass
                if (world.TryGetElement(new Point(x, surfaceY), Layer.Foreground, out ElementIndex index) &&
                    index == ElementIndex.Grass)
                {
                    // 25% chance to attempt tree placement
                    if (SSRandom.Chance(25))
                    {
                        // Choose tree params with limited ranges
                        int trunkHeight = SSRandom.Range(6, 12);
                        int trunkThickness = 1;
                        int crownRadius = SSRandom.Range(2, 4);

                        // Pass world point one above surface so tree sits on top
                        Point treeBase = new(x, surfaceY - 1);
                        TreeGenerator.Start(world, treeBase, trunkHeight, trunkThickness, crownRadius);
                    }
                }
            }
        }
    }
}
