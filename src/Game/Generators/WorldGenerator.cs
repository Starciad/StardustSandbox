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
        private static readonly ElementIndex[] undergroundElements =
        [
            ElementIndex.Stone,
            ElementIndex.Iron
        ];

        internal static void Start(ActorManager actorManager, World world, in WorldGenerationPreset preset)
        {
            int width = world.Information.Size.X;
            int height = world.Information.Size.Y;

            actorManager.Reset();
            world.StartNew();

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

            GenerateTerrain(world, width, height, amplitudeRange, flags);
            GenerateOceans(world, width, height);

            if (flags.HasFlag(WorldGenerationFlags.HasTrees))
            {
                GenerateTrees(world, width, height);
            }
        }

        private static int GetTerrainStartIndex(World world, in int positionX, in int height)
        {
            for (int y = 0; y < height; y++)
            {
                if (world.GetElement(new(positionX, y), Layer.Foreground) is not ElementIndex.None)
                {
                    return y;
                }
            }

            return height;
        }

        private static void GenerateTerrain(World world, in int width, in int height, in Range amplitudeRange, in WorldGenerationFlags flags)
        {
            int generationPositionY = (int)PercentageMath.PercentageOfValue(height, 60.0f);

            for (int x = 0; x < width; x++)
            {
                int startTerrainIndex = generationPositionY;
                int movablePointerY = startTerrainIndex;
                
                int depthLevel = 0;
                int depthLevelLimit = height - startTerrainIndex;

                int grassLayer = SSRandom.Range(2, 4);
                int dirtLayer = grassLayer + SSRandom.Range(4, 8);

                for (int y = movablePointerY; y < height; y++)
                {
                    if (depthLevel <= grassLayer)
                    {
                        world.InstantiateElement(new(x, y), Layer.Foreground, ElementIndex.Grass);
                        world.InstantiateElement(new(x, y), Layer.Background, ElementIndex.Grass);
                    }
                    else if (depthLevel <= dirtLayer)
                    {
                        world.InstantiateElement(new(x, y), Layer.Foreground, ElementIndex.Dirt);
                        world.InstantiateElement(new(x, y), Layer.Background, ElementIndex.Dirt);
                    }
                    else if (depthLevel <= (int)PercentageMath.PercentageOfValue(depthLevelLimit, 80.0f))
                    {
                        world.InstantiateElement(new(x, y), Layer.Foreground, undergroundElements.GetRandomItem());
                        world.InstantiateElement(new(x, y), Layer.Background, undergroundElements.GetRandomItem());
                    }
                    else
                    {
                        world.InstantiateElement(new(x, y), Layer.Foreground, ElementIndex.Obsidian);
                        world.InstantiateElement(new(x, y), Layer.Background, ElementIndex.Obsidian);
                    }

                    depthLevel++;
                }

                generationPositionY += SSRandom.Range(-1, 1);

                if (generationPositionY < amplitudeRange.Start.Value)
                {
                    generationPositionY = amplitudeRange.Start.Value;
                }
                else if (generationPositionY > amplitudeRange.End.Value)
                {
                    generationPositionY = amplitudeRange.End.Value;
                }
            }
        }

        private static void GenerateOceans(World world, in int width, in int height)
        {
            int leftOceanPointX = (int)PercentageMath.PercentageOfValue(width, 5.0f);
            int rightOceanPointX = (int)PercentageMath.PercentageOfValue(width, 95.0f);

            int oceansRadius = (int)PercentageMath.PercentageOfValue(height, 20.0f);
            int sandRadius = oceansRadius + (int)PercentageMath.PercentageOfValue(height, 10.0f);

            int leftStartTerrainIndex = GetTerrainStartIndex(world, leftOceanPointX, height);
            int rightStartTerrainIndex = GetTerrainStartIndex(world, rightOceanPointX, height);

            // Geneerate sand around oceans
            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(new(leftOceanPointX, leftStartTerrainIndex), sandRadius))
            {
                world.ReplaceElement(point, Layer.Foreground, ElementIndex.Sand);
                world.ReplaceElement(point, Layer.Background, ElementIndex.Sand);
            }

            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(new(rightOceanPointX, rightStartTerrainIndex), sandRadius))
            {
                world.ReplaceElement(point, Layer.Foreground, ElementIndex.Sand);
                world.ReplaceElement(point, Layer.Background, ElementIndex.Sand);
            }

            // Generate oceans
            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(new(leftOceanPointX, leftStartTerrainIndex), oceansRadius))
            {
                world.ReplaceElement(point, Layer.Foreground, ElementIndex.Water);
                world.ReplaceElement(point, Layer.Background, ElementIndex.Water);
            }

            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(new(rightOceanPointX, rightStartTerrainIndex), oceansRadius))
            {
                world.ReplaceElement(point, Layer.Foreground, ElementIndex.Water);
                world.ReplaceElement(point, Layer.Background, ElementIndex.Water);
            }
        }

        private static void GenerateTrees(World world, in int width, in int height)
        {
            for (int x = 0; x < width; x++)
            {
                int startTerrainIndex = GetTerrainStartIndex(world, x, height);

                if (world.TryGetElement(new(x, startTerrainIndex), Layer.Foreground, out ElementIndex index) &&
                    index is ElementIndex.Grass &&
                    SSRandom.Chance(25))
                {
                    TreeGenerator.Start(world, new(x, startTerrainIndex - 1), SSRandom.Range(6, 12), 1, SSRandom.Range(2, 4));
                }
            }
        }
    }
}
