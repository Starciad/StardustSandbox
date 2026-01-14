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

using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace StardustSandbox.Mathematics.Primitives
{
    [DataContract]
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    internal struct RectangleF : IEquatable<RectangleF>
    {
        internal static RectangleF Empty => new();

        internal readonly float Left => this.X;
        internal readonly float Right => this.X + this.Width;
        internal readonly float Top => this.Y;
        internal readonly float Bottom => this.Y + this.Height;
        internal readonly bool IsEmpty => this.Width == 0 && this.Height == 0 && this.X == 0 && this.Y == 0;

        internal Vector2 Location
        {
            readonly get => new(this.X, this.Y);
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        internal Vector2 Size
        {
            readonly get => new(this.Width, this.Height);
            set
            {
                this.Width = value.X;
                this.Height = value.Y;
            }
        }

        internal readonly Vector2 Center => new(this.X + (this.Width / 2), this.Y + (this.Height / 2));
        internal readonly string DebugDisplayString => this.X + "  " + this.Y + "  " + this.Width + "  " + this.Height;

        [DataMember]
        internal float X;

        [DataMember]
        internal float Y;

        [DataMember]
        internal float Width;

        [DataMember]
        internal float Height;

        internal RectangleF(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        internal RectangleF(Vector2 location, Vector2 size)
        {
            this.X = location.X;
            this.Y = location.Y;
            this.Width = size.X;
            this.Height = size.Y;
        }

        public static bool operator ==(RectangleF a, RectangleF b)
        {
            return a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
        }

        public static bool operator !=(RectangleF a, RectangleF b)
        {
            return !(a == b);
        }

        internal readonly bool Contains(float x, float y)
        {
            return this.X <= x && x < this.X + this.Width && this.Y <= y && y < this.Y + this.Height;
        }

        internal readonly bool Contains(Vector2 value)
        {
            return this.X <= value.X && value.X < this.X + this.Width && this.Y <= value.Y && value.Y < this.Y + this.Height;
        }

        internal readonly void Contains(ref Vector2 value, out bool result)
        {
            result = this.X <= value.X && value.X < this.X + this.Width && this.Y <= value.Y && value.Y < this.Y + this.Height;
        }

        internal readonly bool Contains(RectangleF value)
        {
            return this.X <= value.X && value.X + value.Width <= this.X + this.Width && this.Y <= value.Y && value.Y + value.Height <= this.Y + this.Height;
        }

        internal readonly void Contains(ref RectangleF value, out bool result)
        {
            result = this.X <= value.X && value.X + value.Width <= this.X + this.Width && this.Y <= value.Y && value.Y + value.Height <= this.Y + this.Height;
        }

        public override readonly bool Equals(object obj)
        {
            return obj is RectangleF f && this == f;
        }

        internal readonly bool Equals(RectangleF other)
        {
            return this == other;
        }

        public override readonly int GetHashCode()
        {
            return (((((((17 * 23) + this.X.GetHashCode()) * 23) + this.Y.GetHashCode()) * 23) + this.Width.GetHashCode()) * 23) + this.Height.GetHashCode();
        }

        internal void Inflate(float horizontalAmount, float verticalAmount)
        {
            this.X -= horizontalAmount;
            this.Y -= verticalAmount;
            this.Width += horizontalAmount * 2;
            this.Height += verticalAmount * 2;
        }

        internal readonly bool Intersects(RectangleF value)
        {
            return value.Left < this.Right && this.Left < value.Right && value.Top < this.Bottom && this.Top < value.Bottom;
        }

        internal readonly void Intersects(ref RectangleF value, out bool result)
        {
            result = value.Left < this.Right && this.Left < value.Right && value.Top < this.Bottom && this.Top < value.Bottom;
        }

        internal static RectangleF Intersect(RectangleF value1, RectangleF value2)
        {
            Intersect(ref value1, ref value2, out RectangleF result);
            return result;
        }

        internal static void Intersect(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
        {
            if (value1.Intersects(value2))
            {
                float num = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
                float num2 = Math.Max(value1.X, value2.X);
                float num3 = Math.Max(value1.Y, value2.Y);
                float num4 = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
                result = new(num2, num3, num - num2, num4 - num3);
            }
            else
            {
                result = new(0, 0, 0, 0);
            }
        }

        internal void Offset(float offsetX, float offsetY)
        {
            this.X += offsetX;
            this.Y += offsetY;
        }

        internal void Offset(Vector2 amount)
        {
            this.X += amount.X;
            this.Y += amount.Y;
        }

        public override readonly string ToString()
        {
            return "{X:" + this.X + " Y:" + this.Y + " Width:" + this.Width + " Height:" + this.Height + "}";
        }

        internal static RectangleF Union(RectangleF value1, RectangleF value2)
        {
            float num = Math.Min(value1.X, value2.X);
            float num2 = Math.Min(value1.Y, value2.Y);
            return new(num, num2, Math.Max(value1.Right, value2.Right) - num, Math.Max(value1.Bottom, value2.Bottom) - num2);
        }

        internal static void Union(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
        {
            result.X = Math.Min(value1.X, value2.X);
            result.Y = Math.Min(value1.Y, value2.Y);
            result.Width = Math.Max(value1.Right, value2.Right) - result.X;
            result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;
        }

        internal readonly void Deconstruct(out float x, out float y, out float width, out float height)
        {
            x = this.X;
            y = this.Y;
            width = this.Width;
            height = this.Height;
        }

        readonly bool IEquatable<RectangleF>.Equals(RectangleF other)
        {
            return Equals(other);
        }
    }
}

