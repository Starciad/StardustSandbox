using Microsoft.Xna.Framework;

using System;

namespace PixelDust.Game.Mathematics
{
    public struct Size2 : IEquatable<Size2>
    {
        public static readonly Size2 Empty = new();
        public readonly bool IsEmpty => this.Width == 0 && this.Height == 0;

        public float Width;
        public float Height;

        public Size2(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static implicit operator Size2(Point point)
        {
            return new Size2(point.X, point.Y);
        }
        public static implicit operator Point(Size2 size)
        {
            return new Point((int)size.Width, (int)size.Height);
        }

        public static explicit operator Size2(Size2Int size)
        {
            return new Size2(size.Width, size.Height);
        }

        public static Size2 operator +(Size2 first, Size2 second)
        {
            return Add(first, second);
        }
        public static Size2 operator -(Size2 first, Size2 second)
        {
            return Subtract(first, second);
        }
        public static Size2 operator *(Size2 size, float value)
        {
            return new Size2(size.Width * value, size.Height * value);
        }
        public static Size2 operator /(Size2 size, float value)
        {
            return new Size2(size.Width / value, size.Height / value);
        }

        public static bool operator ==(Size2 first, Size2 second)
        {
            return first.Equals(ref second);
        }
        public static bool operator !=(Size2 first, Size2 second)
        {
            return !(first == second);
        }

        public static Size2 Add(Size2 first, Size2 second)
        {
            Size2 size;
            size.Width = first.Width + second.Width;
            size.Height = first.Height + second.Height;
            return size;
        }
        public static Size2 Subtract(Size2 first, Size2 second)
        {
            Size2 size;
            size.Width = first.Width - second.Width;
            size.Height = first.Height - second.Height;
            return size;
        }

        public override readonly string ToString()
        {
            return $"{{ Width: {this.Width}, Height: {this.Height} }}";
        }
        public override readonly int GetHashCode()
        {
            unchecked
            {
                return this.Width.GetHashCode() * 397 ^ this.Height.GetHashCode();
            }
        }

        public readonly bool Equals(Size2 size)
        {
            return Equals(ref size);
        }
        public readonly bool Equals(ref Size2 size)
        {
            return this.Width == size.Width && this.Height == size.Height;
        }
        public override readonly bool Equals(object obj)
        {
            return obj is Size2 @float && Equals(@float);
        }
    }
}