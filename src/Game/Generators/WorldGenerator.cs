using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.Generators;
using StardustSandbox.Enums.World;
using StardustSandbox.Mathematics;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Generators
{
    internal static class WorldGenerator
    {
        internal static void Start(World world, in WorldGenerationPreset preset)
        {
            int width = world.Information.Size.X;
            int height = world.Information.Size.Y;

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
        }

        private static void GenerateTerrain(World world, in int width, in int height, in Range amplitudeRange, in WorldGenerationFlags flags)
        {
            int generationPositionY = (int)PercentageMath.PercentageOfValue(height, 60.0f);

            for (int x = 0; x < width; x++)
            {
                int startTerrainIndex = generationPositionY;

                if (flags.HasFlag(WorldGenerationFlags.HasTrees) && SSRandom.Chance(15, 150))
                {
                    TreeGenerator.Start(world, new(x, startTerrainIndex - 1), SSRandom.Range(6, 9), 1, SSRandom.Range(2, 4));
                }

                int movablePointerY = startTerrainIndex;
                
                int depthLevel = 0;
                int depthLevelLimit = height - startTerrainIndex;

                for (int y = movablePointerY; y < height; y++)
                {
                    if (depthLevel <= 2)
                    {
                        world.InstantiateElement(new(x, y), Layer.Foreground, ElementIndex.Grass);
                    }
                    else if (depthLevel <= 5)
                    {
                        world.InstantiateElement(new(x, y), Layer.Foreground, ElementIndex.Dirt);
                    }
                    else if (depthLevel <= (int)PercentageMath.PercentageOfValue(depthLevelLimit, 80.0f))
                    {
                        world.InstantiateElement(new(x, y), Layer.Foreground, ElementIndex.Stone);
                    }
                    else
                    {
                        world.InstantiateElement(new(x, y), Layer.Foreground, ElementIndex.Obsidian);
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
    }
}
