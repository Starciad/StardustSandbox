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

