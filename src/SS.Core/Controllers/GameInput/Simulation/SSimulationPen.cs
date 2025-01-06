using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.Enums.World;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Controllers.GameInput.Simulation
{
    public sealed class SSimulationPen
    {
        public sbyte Size
        {
            get => this.size;
            set => this.size = sbyte.Clamp(value, SInputConstants.PEN_MIN_SIZE, SInputConstants.PEN_MAX_SIZE);
        }
        public SPenTool Tool { get; set; }
        public SWorldLayer Layer { get; set; }
        public SPenShape Shape { get; set; }

        private sbyte size = SInputConstants.PEN_MIN_SIZE;

        public SSimulationPen()
        {
            this.Tool = SPenTool.Pencil;
            this.Layer = SWorldLayer.Foreground;
            this.Shape = SPenShape.Circle;
        }

        public IEnumerable<Point> GetShapePoints(Point position)
        {
            return this.Shape switch
            {
                SPenShape.Circle => FillCirclePositions(position, this.Size),
                SPenShape.Square => FillSquarePositions(position, this.Size),
                SPenShape.Triangle => FillTrianglePositions(position, this.Size),
                _ => throw new NotSupportedException($"Shape {this.Shape} is not supported."),
            };
        }

        private static IEnumerable<Point> FillCirclePositions(Point position, int radius)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if ((x * x) + (y * y) <= radius * radius)
                    {
                        yield return new(position.X + x, position.Y + y);
                    }
                }
            }
        }

        private static IEnumerable<Point> FillSquarePositions(Point position, int radius)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    yield return new(position.X + x, position.Y + y);
                }
            }
        }

        private static IEnumerable<Point> FillTrianglePositions(Point position, int radius)
        {
            for (int y = 0; y <= radius; y++)
            {
                int rowWidth = radius - y;
                for (int x = -rowWidth; x <= rowWidth; x++)
                {
                    yield return new(position.X + x, position.Y - y + (radius / 2));
                }
            }
        }
    }
}
