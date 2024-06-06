using Microsoft.Xna.Framework;

using System;

namespace StardustSandbox.Game.Mathematics
{
    public struct SSize2 : IEquatable<SSize2>
    {
        public static readonly SSize2 Empty = new();
        public readonly bool IsEmpty => this.Width == 0 && this.Height == 0;

        public static SSize2 Zero => new(0, 0);
        public static SSize2 One => new(1, 1);

        public float Width;
        public float Height;

        public SSize2(float value)
        {
            this.Width = value;
            this.Height = value;
        }
        public SSize2(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static implicit operator SSize2(Point point)
        {
            return new SSize2(point.X, point.Y);
        }
        public static implicit operator Point(SSize2 size)
        {
            return new Point((int)size.Width, (int)size.Height);
        }

        public static SSize2 operator +(SSize2 first, SSize2 second)
        {
            return Add(first, second);
        }
        public static SSize2 operator -(SSize2 first, SSize2 second)
        {
            return Subtract(first, second);
        }
        public static SSize2 operator *(SSize2 size, float value)
        {
            return new SSize2(size.Width * value, size.Height * value);
        }
        public static SSize2 operator /(SSize2 size, float value)
        {
            return new SSize2(size.Width / value, size.Height / value);
        }

        public static bool operator ==(SSize2 first, SSize2 second)
        {
            return first.Equals(ref second);
        }
        public static bool operator !=(SSize2 first, SSize2 second)
        {
            return !(first == second);
        }

        public static SSize2 Add(SSize2 first, SSize2 second)
        {
            SSize2 size;
            size.Width = first.Width + second.Width;
            size.Height = first.Height + second.Height;
            return size;
        }
        public static SSize2 Subtract(SSize2 first, SSize2 second)
        {
            SSize2 size;
            size.Width = first.Width - second.Width;
            size.Height = first.Height - second.Height;
            return size;
        }

        public readonly Point ToPoint()
        {
            return new Point((int)this.Width, (int)this.Height);
        }
        public readonly Vector2 ToVector2()
        {
            return new Vector2(this.Width, this.Height);
        }
        public override readonly string ToString()
        {
            return $"{{ Width: {this.Width}, Height: {this.Height} }}";
        }
        public override readonly int GetHashCode()
        {
            unchecked
            {
                return (this.Width.GetHashCode() * 397) ^ this.Height.GetHashCode();
            }
        }

        public readonly bool Equals(SSize2 size)
        {
            return Equals(ref size);
        }
        public readonly bool Equals(ref SSize2 size)
        {
            return this.Width == size.Width && this.Height == size.Height;
        }
        public override readonly bool Equals(object obj)
        {
            return obj is SSize2 @float && Equals(@float);
        }
    }
}