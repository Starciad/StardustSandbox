using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Mathematics.Geometry;

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
                SPenShape.Circle => SShapePointGenerator.GenerateCirclePoints(position, this.Size),
                SPenShape.Square => SShapePointGenerator.GenerateSquarePoints(position, this.Size),
                SPenShape.Triangle => SShapePointGenerator.GenerateTrianglePoints(position, this.Size),
                _ => throw new NotSupportedException($"Shape {this.Shape} is not supported."),
            };
        }
    }
}
