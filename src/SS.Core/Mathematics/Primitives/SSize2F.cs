using Microsoft.Xna.Framework;

using System;

namespace StardustSandbox.Core.Mathematics.Primitives
{
    public struct SSize2F : IEquatable<SSize2F>
    {
        public static readonly SSize2F Empty = new();
        public readonly bool IsEmpty => this.Width == 0 && this.Height == 0;

        public static SSize2F Zero => new(0, 0);
        public static SSize2F One => new(1, 1);

        public float Width;
        public float Height;

        public SSize2F()
        {
            this.Width = 0f;
            this.Height = 0f;
        }
        public SSize2F(float value)
        {
            this.Width = value;
            this.Height = value;
        }
        public SSize2F(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static implicit operator SSize2F(SSize2 value)
        {
            return new SSize2F(value.Width, value.Height);
        }
        public static implicit operator SSize2F(Point value)
        {
            return new SSize2F(value.X, value.Y);
        }
        public static implicit operator Point(SSize2F value)
        {
            return new Point((int)value.Width, (int)value.Height);
        }

        public static SSize2F operator +(SSize2F first, SSize2F second)
        {
            return Add(first, second);
        }
        public static SSize2F operator -(SSize2F first, SSize2F second)
        {
            return Subtract(first, second);
        }
        public static SSize2F operator *(SSize2F size, float value)
        {
            return new SSize2F(size.Width * value, size.Height * value);
        }
        public static SSize2F operator /(SSize2F size, float value)
        {
            return new SSize2F(size.Width / value, size.Height / value);
        }

        public static bool operator ==(SSize2F first, SSize2F second)
        {
            return first.Equals(ref second);
        }
        public static bool operator !=(SSize2F first, SSize2F second)
        {
            return !(first == second);
        }

        public static SSize2F Add(SSize2F first, SSize2F second)
        {
            SSize2F size;
            size.Width = first.Width + second.Width;
            size.Height = first.Height + second.Height;
            return size;
        }
        public static SSize2F Subtract(SSize2F first, SSize2F second)
        {
            SSize2F size;
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

        public readonly bool Equals(SSize2F size)
        {
            return Equals(ref size);
        }
        public readonly bool Equals(ref SSize2F size)
        {
            return this.Width == size.Width && this.Height == size.Height;
        }
        public override readonly bool Equals(object obj)
        {
            return obj is SSize2F @float && Equals(@float);
        }
    }
}