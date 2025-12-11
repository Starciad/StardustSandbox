using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.World;
using StardustSandbox.Mathematics;

using System;
using System.Collections.Generic;

namespace StardustSandbox.InputSystem.Game.Simulation
{
    internal sealed class Pen
    {
        internal sbyte Size
        {
            get => this.size;
            set => this.size = sbyte.Clamp(value, InputConstants.PEN_MIN_SIZE, InputConstants.PEN_MAX_SIZE);
        }

        internal PenTool Tool { get; set; }
        internal Layer Layer { get; set; }
        internal PenShape Shape { get; set; }

        private sbyte size = InputConstants.PEN_MIN_SIZE;

        internal Pen()
        {
            this.Tool = PenTool.Pencil;
            this.Layer = Layer.Foreground;
            this.Shape = PenShape.Circle;
        }

        internal IEnumerable<Point> GetShapePoints(Point position)
        {
            return this.Shape switch
            {
                PenShape.Circle => ShapePointGenerator.GenerateCirclePoints(position, this.Size),
                PenShape.Square => ShapePointGenerator.GenerateSquarePoints(position, this.Size),
                PenShape.Triangle => ShapePointGenerator.GenerateTrianglePoints(position, this.Size),
                _ => throw new NotSupportedException($"Shape {this.Shape} is not supported."),
            };
        }
    }
}
