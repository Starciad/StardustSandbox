using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.GameInput.Pen;

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
        public SPenLayer Layer { get; set; }
        public SPenShape Shape { get; set; }

        private sbyte size = SInputConstants.PEN_MIN_SIZE;

        public Point[] GetPenShapePoints(Point position)
        {
            return this.Shape switch
            {
                SPenShape.Circle => GetCirclePositions(position, this.Size),
                SPenShape.Square => GetSquarePositions(position, this.Size),
                SPenShape.Triangle => GetTrianglePositions(position, this.Size),
                _ => throw new NotSupportedException($"Shape {this.Shape} is not supported.")
            };
        }

        private static Point[] GetCirclePositions(Point position, int radius)
        {
            List<Point> points = [];

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if ((x * x) + (y * y) <= radius * radius)
                    {
                        points.Add(new(position.X + x, position.Y + y));
                    }
                }
            }

            return [.. points];
        }

        private static Point[] GetSquarePositions(Point position, int radius)
        {
            List<Point> points = [];

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    points.Add(new(position.X + x, position.Y + y));
                }
            }

            return [.. points];
        }

        private static Point[] GetTrianglePositions(Point position, int radius)
        {
            List<Point> points = [];

            for (int y = 0; y <= radius; y++)
            {
                int rowWidth = radius - y;
                for (int x = -rowWidth; x <= rowWidth; x++)
                {
                    points.Add(new(position.X + x, position.Y - y + radius / 2));
                }
            }

            return [.. points];
        }
    }
}
