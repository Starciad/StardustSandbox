using Microsoft.Xna.Framework;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace PixelDust.Game.Mathematics
{
    [DataContract]
    public struct Vector2Int : IEquatable<Vector2Int>
    {
        #region Private Fields

        private static readonly Vector2Int zeroVector = new(0, 0);
        private static readonly Vector2Int unitVector = new(1, 1);
        private static readonly Vector2Int unitXVector = new(1, 0);
        private static readonly Vector2Int unitYVector = new(0, 1);

        #endregion

        #region Public Fields

        [DataMember] public int X;
        [DataMember] public int Y;

        #endregion

        #region Properties

        public static Vector2Int Zero => zeroVector;
        public static Vector2Int One => unitVector;
        public static Vector2Int UnitX => unitXVector;
        public static Vector2Int UnitY => unitYVector;

        #endregion

        #region Constructors

        public Vector2Int(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public Vector2Int(int value)
        {
            this.X = value;
            this.Y = value;
        }

        #endregion

        #region Public Methods

        public static Vector2Int Add(Vector2Int value1, Vector2Int value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }
        public static Vector2Int Subtract(Vector2Int value1, Vector2Int value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }
        public static Vector2Int Multiply(Vector2Int value1, Vector2Int value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }
        public static Vector2Int Multiply(Vector2Int value1, int scaleFactor)
        {
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;
            return value1;
        }
        public static Vector2Int Divide(Vector2Int value1, Vector2Int value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }
        public static Vector2Int Divide(Vector2Int value1, int divider)
        {
            int factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        public static Vector2Int CatmullRom(Vector2Int value1, Vector2Int value2, Vector2Int value3, Vector2Int value4, int amount)
        {
            return new Vector2Int(
                (int)MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
                (int)MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
        }
        public void Ceiling()
        {
            this.X = (int)MathF.Ceiling(this.X);
            this.Y = (int)MathF.Ceiling(this.Y);
        }
        public static Vector2Int Ceiling(Vector2Int value)
        {
            value.X = (int)MathF.Ceiling(value.X);
            value.Y = (int)MathF.Ceiling(value.Y);
            return value;
        }
        public static Vector2Int Clamp(Vector2Int value1, Vector2Int min, Vector2Int max)
        {
            return new Vector2Int(
                MathHelper.Clamp(value1.X, min.X, max.X),
                MathHelper.Clamp(value1.Y, min.Y, max.Y));
        }
        public static float Distance(Vector2Int value1, Vector2Int value2)
        {
            int v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return MathF.Sqrt(v1 * v1 + v2 * v2);
        }
        public static float DistanceSquared(Vector2Int value1, Vector2Int value2)
        {
            int v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return v1 * v1 + v2 * v2;
        }

        public static int Dot(Vector2Int value1, Vector2Int value2)
        {
            return value1.X * value2.X + value1.Y * value2.Y;
        }
        public void Floor()
        {
            this.X = (int)MathF.Floor(this.X);
            this.Y = (int)MathF.Floor(this.Y);
        }
        public static Vector2Int Floor(Vector2Int value)
        {
            value.X = (int)MathF.Floor(value.X);
            value.Y = (int)MathF.Floor(value.Y);
            return value;
        }

        public static Vector2Int Max(Vector2Int value1, Vector2Int value2)
        {
            return new Vector2Int(value1.X > value2.X ? value1.X : value2.X,
                                  value1.Y > value2.Y ? value1.Y : value2.Y);
        }
        public static Vector2Int Min(Vector2Int value1, Vector2Int value2)
        {
            return new Vector2Int(value1.X < value2.X ? value1.X : value2.X,
                               value1.Y < value2.Y ? value1.Y : value2.Y);
        }
        public static Vector2Int Negate(Vector2Int value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        public readonly int Length()
        {
            return (int)MathF.Sqrt(this.X * this.X + this.Y * this.Y);
        }
        public readonly int LengthSquared()
        {
            return this.X * this.X + this.Y * this.Y;
        }

        public readonly void Deconstruct(out int x, out int y)
        {
            x = this.X;
            y = this.Y;
        }

        #endregion

        #region Operators
        public static implicit operator Vector2Int(Vector2 value)
        {
            return new Vector2Int((int)value.X, (int)value.Y);
        }
        public static implicit operator Vector2Int(Vector3 value)
        {
            return new Vector2Int((int)value.X, (int)value.Y);
        }

        public static explicit operator Vector2(Vector2Int value)
        {
            return new Vector2(value.X, value.Y);
        }
        public static explicit operator Vector3(Vector2Int value)
        {
            return new Vector3(value.X, value.Y, 0);
        }

        // Equals
        public static bool operator ==(Vector2Int value1, Vector2Int value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }
        public static bool operator !=(Vector2Int value1, Vector2Int value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }

        // Calc
        public static Vector2Int operator +(Vector2Int value1, Vector2Int value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }
        public static Vector2Int operator -(Vector2Int value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }
        public static Vector2Int operator -(Vector2Int value1, Vector2Int value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }
        public static Vector2Int operator *(Vector2Int value1, Vector2Int value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }
        public static Vector2Int operator *(Vector2Int value, int scaleFactor)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }
        public static Vector2Int operator *(int scaleFactor, Vector2Int value)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator /(Vector2Int value1, Vector2Int value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator /(Vector2Int value1, int divider)
        {
            int factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        #endregion

        #region Conversions

        public readonly Point ToPoint()
        {
            return new Point(this.X, this.Y);
        }
        public override readonly string ToString()
        {
            return "{X:" + this.X + " Y:" + this.Y + "}";
        }

        #endregion

        #region System

        public override readonly bool Equals(object obj)
        {
            return obj is Vector2Int @int && Equals(@int);
        }
        public readonly bool Equals(Vector2Int other)
        {
            return this.X == other.X && this.Y == other.Y;
        }
        public override readonly int GetHashCode()
        {
            unchecked
            {
                return this.X.GetHashCode() * 397 ^ this.Y.GetHashCode();
            }
        }

        #endregion
    }
}