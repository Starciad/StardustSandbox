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

using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Generators;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.WorldSystem;
using StardustSandbox.Core.WorldSystem.Components;

using System;

namespace StardustSandbox.Core.Generators
{
    internal sealed class WorldGenerator
    {
        internal WorldGenerationTheme Theme { get; set; }
        internal WorldGenerationSettings Settings { get; set; }
        internal WorldGenerationContents Contents { get; set; }

        private int[] heightMap;

        private Range amplitudeRange;
        private int smoothness;

        private int width;
        private int height;

        private readonly ElementContext elementContext;
        private readonly GameHandler gameHandler;
        private readonly TileMap tileMap;

        internal WorldGenerator(GameHandler gameHandler, World world)
        {
            this.elementContext = new(world);
            this.gameHandler = gameHandler;
            this.tileMap = world.TileMap;
        }

        internal void Start()
        {
            this.width = this.tileMap.Width;
            this.height = this.tileMap.Height;

            this.gameHandler.Reset();

            ConfigureThemeParameters();

            if (this.Settings.HasFlag(WorldGenerationSettings.GenerateForeground))
            {
                GenerateHeightMap();
                StartGenerationProcess(Layer.Foreground);
            }

            if (this.Settings.HasFlag(WorldGenerationSettings.GenerateBackground))
            {
                GenerateHeightMap();
                StartGenerationProcess(Layer.Background);
            }
        }

        private void ConfigureThemeParameters()
        {
            switch (this.Theme)
            {
                case WorldGenerationTheme.Plain:
                    this.amplitudeRange = new(
                        (int)PercentageMath.PercentageOfValue(this.height, 25.0f),
                        (int)PercentageMath.PercentageOfValue(this.height, 45.0f)
                    );
                    this.smoothness = 4;
                    break;

                case WorldGenerationTheme.Desert:
                    this.amplitudeRange = new(
                        (int)PercentageMath.PercentageOfValue(this.height, 20.0f),
                        (int)PercentageMath.PercentageOfValue(this.height, 50.0f)
                    );
                    this.smoothness = 5;
                    break;

                case WorldGenerationTheme.Snow:
                    this.amplitudeRange = new(
                        (int)PercentageMath.PercentageOfValue(this.height, 40.0f),
                        (int)PercentageMath.PercentageOfValue(this.height, 70.0f)
                    );
                    this.smoothness = 3;
                    break;

                case WorldGenerationTheme.Volcanic:
                    this.amplitudeRange = new(
                        (int)PercentageMath.PercentageOfValue(this.height, 10.0f),
                        (int)PercentageMath.PercentageOfValue(this.height, 40.0f)
                    );
                    this.smoothness = 2;
                    break;

                case WorldGenerationTheme.Ocean:
                    this.amplitudeRange = new(
                        (int)PercentageMath.PercentageOfValue(this.height, 30.0f),
                        (int)PercentageMath.PercentageOfValue(this.height, 50.0f)
                    );
                    this.smoothness = 1;
                    break;

                default:
                    this.amplitudeRange = new(
                        (int)PercentageMath.PercentageOfValue(this.height, 30.0f),
                        (int)PercentageMath.PercentageOfValue(this.height, 60.0f)
                    );
                    this.smoothness = 4;
                    break;
            }
        }

        private void StartGenerationProcess(Layer layer)
        {
            GenerateTerrain(layer);

            if (this.Contents.HasFlag(WorldGenerationContents.HasOceans))
            {
                GenerateOceans(layer);
            }

            if (this.Contents.HasFlag(WorldGenerationContents.HasVegetation))
            {
                GenerateTrees(layer);
            }

            if (this.Contents.HasFlag(WorldGenerationContents.HasClouds))
            {
                GenerateClouds(layer);
            }
        }

        private void GenerateHeightMap()
        {
            this.heightMap = new int[this.width];

            int baseline = (int)PercentageMath.PercentageOfValue(this.height, 60.0f);
            this.heightMap[0] = baseline;

            for (int x = 1; x < this.width; x++)
            {
                int delta = Randomness.Random.Range(-2, 2);
                int candidate = this.heightMap[x - 1] + delta;

                if (candidate < this.amplitudeRange.Start.Value)
                {
                    candidate = this.amplitudeRange.Start.Value;
                }
                else if (candidate > this.amplitudeRange.End.Value)
                {
                    candidate = this.amplitudeRange.End.Value;
                }

                this.heightMap[x] = candidate;
            }

            // Apply a few smoothing passes (moving average) to remove jitter
            for (int pass = 0; pass < this.smoothness; pass++)
            {
                int[] temp = new int[this.width];

                for (int x = 0; x < this.width; x++)
                {
                    int left = (x - 1 >= 0) ? this.heightMap[x - 1] : this.heightMap[x];
                    int right = (x + 1 < this.width) ? this.heightMap[x + 1] : this.heightMap[x];

                    // weighted average: center*2 + left + right
                    int smoothed = ((2 * this.heightMap[x]) + left + right) / 4;

                    if (smoothed < this.amplitudeRange.Start.Value)
                    {
                        smoothed = this.amplitudeRange.Start.Value;
                    }
                    else if (smoothed > this.amplitudeRange.End.Value)
                    {
                        smoothed = this.amplitudeRange.End.Value;
                    }

                    temp[x] = smoothed;
                }

                // copy back
                for (int x = 0; x < this.width; x++)
                {
                    this.heightMap[x] = temp[x];
                }
            }
        }

        private void GenerateTerrain(Layer layer)
        {
            int width = this.heightMap.Length;
            int height = this.tileMap.Height;

            for (int x = 0; x < width; x++)
            {
                int startY = this.heightMap[x];

                // Randomize layer thickness per column but keep it bounded
                int surfaceThickness = Randomness.Random.Range(2, 4);
                int subsurfaceThickness = surfaceThickness + Randomness.Random.Range(4, 8);

                int depthLevelLimit = height - startY;
                int deepThreshold = (int)PercentageMath.PercentageOfValue(depthLevelLimit, 80.0f);

                // Choose element types according to selected theme
                ElementIndex surfaceElement;
                ElementIndex subsurfaceElement;
                ElementIndex rockElement;
                ElementIndex abyssElement;

                switch (this.Theme)
                {
                    case WorldGenerationTheme.Desert:
                        surfaceElement = ElementIndex.Sand;
                        subsurfaceElement = ElementIndex.Sand;
                        rockElement = ElementIndex.Stone;
                        abyssElement = ElementIndex.Obsidian;
                        break;

                    case WorldGenerationTheme.Snow:
                        surfaceElement = ElementIndex.Snow;
                        subsurfaceElement = ElementIndex.Snow;
                        rockElement = ElementIndex.Ice;
                        abyssElement = ElementIndex.Obsidian;
                        break;

                    case WorldGenerationTheme.Volcanic:
                        surfaceElement = ElementIndex.Ash;
                        subsurfaceElement = ElementIndex.Obsidian;
                        rockElement = ElementIndex.Lava;
                        abyssElement = ElementIndex.Obsidian;
                        break;

                    case WorldGenerationTheme.Ocean:
                        surfaceElement = ElementIndex.Saltwater;
                        subsurfaceElement = ElementIndex.Saltwater;
                        rockElement = ElementIndex.Saltwater;
                        abyssElement = ElementIndex.Stone;
                        break;

                    case WorldGenerationTheme.Plain:
                    default:
                        surfaceElement = ElementIndex.Grass;
                        subsurfaceElement = ElementIndex.Dirt;
                        rockElement = ElementIndex.Stone;
                        abyssElement = ElementIndex.Obsidian;
                        break;
                }

                for (int y = startY; y < height; y++)
                {
                    int relativeDepth = y - startY;
                    ElementIndex chosen =
                        relativeDepth <= surfaceThickness ? surfaceElement
                        : relativeDepth <= subsurfaceThickness ? subsurfaceElement
                        : relativeDepth <= deepThreshold ? rockElement : abyssElement;

                    this.tileMap.InstantiateElementIndex(new(x, y), layer, chosen);
                }
            }
        }

        private void GenerateOceans(Layer layer)
        {
            int width = this.heightMap.Length;
            int height = this.tileMap.Height;

            int leftOceanPointX = (int)PercentageMath.PercentageOfValue(width, 5.0f);
            int rightOceanPointX = (int)PercentageMath.PercentageOfValue(width, 95.0f);

            int oceansRadius = (int)PercentageMath.PercentageOfValue(height, 20.0f);
            int sandRadius = oceansRadius + (int)PercentageMath.PercentageOfValue(height, 10.0f);

            int leftStartTerrainIndex = this.heightMap[leftOceanPointX];
            int rightStartTerrainIndex = this.heightMap[rightOceanPointX];

            Point leftCenter = new(leftOceanPointX, leftStartTerrainIndex);
            Point rightCenter = new(rightOceanPointX, rightStartTerrainIndex);

            // Generate sand band first (keeps shoreline consistent)
            foreach (Point point in ShapePointGenerator.EnumerateCirclePoints(leftCenter, sandRadius))
            {
                if (this.tileMap.IsWithinBounds(point))
                {
                    this.tileMap.ReplaceElementIndex(point, layer, ElementIndex.Sand);
                }
            }

            foreach (Point point in ShapePointGenerator.EnumerateCirclePoints(rightCenter, sandRadius))
            {
                if (this.tileMap.IsWithinBounds(point))
                {
                    this.tileMap.ReplaceElementIndex(point, layer, ElementIndex.Sand);
                }
            }

            // Generate water inside the sand band
            foreach (Point point in ShapePointGenerator.EnumerateCirclePoints(leftCenter, oceansRadius))
            {
                if (this.tileMap.IsWithinBounds(point))
                {
                    this.tileMap.ReplaceElementIndex(point, layer, ElementIndex.Saltwater);
                }
            }

            foreach (Point point in ShapePointGenerator.EnumerateCirclePoints(rightCenter, oceansRadius))
            {
                if (this.tileMap.IsWithinBounds(point))
                {
                    this.tileMap.ReplaceElementIndex(point, layer, ElementIndex.Saltwater);
                }
            }
        }

        private void GenerateTrees(Layer layer)
        {
            int width = this.heightMap.Length;
            int height = this.tileMap.Height;

            for (int x = 0; x < width; x++)
            {
                int surfaceY = this.heightMap[x];

                // Ensure we are in world bounds
                if (surfaceY <= 0 || surfaceY >= height)
                {
                    continue;
                }

                // Confirm top element is grass
                if (Randomness.Random.Chance(25) && this.tileMap.TryGetElementIndex(new(x, surfaceY), layer, out ElementIndex index) && index is ElementIndex.Grass)
                {
                    Point origin = new(x, surfaceY - 1);
                    int trunkHeight = Randomness.Random.Range(6, 12);
                    int trunkThickness = 1;
                    int crownRadius = Randomness.Random.Range(2, 4);

                    this.elementContext.Initialize(origin, layer);

                    TreeGenerator.Start(this.elementContext, trunkHeight, trunkThickness, crownRadius);
                }
            }
        }

        private void GenerateClouds(Layer layer)
        {
            int width = this.heightMap.Length;
            int height = this.tileMap.Height;
            int cloudBaseY = (int)PercentageMath.PercentageOfValue(height, 15.0f);

            for (int x = 0; x < width; x++)
            {
                if (Randomness.Random.Chance(10))
                {
                    Point origin = new(x, cloudBaseY);

                    foreach (Point point in ShapePointGenerator.EnumerateCirclePoints(origin, Randomness.Random.Range(3, 6)))
                    {
                        this.tileMap.InstantiateElementIndex(point, layer, ElementIndex.Cloud);
                    }
                }
            }
        }
    }
}
