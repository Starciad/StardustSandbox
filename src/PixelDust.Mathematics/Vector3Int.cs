using Microsoft.Xna.Framework;

using System;
using System.Runtime.Serialization;
using System.Text;

namespace PixelDust.Mathematics
{
    [DataContract]
    public struct Vector3Int : IEquatable<Vector3Int>
    {
        #region Private Fields

        private static readonly Vector3Int zero = new(0, 0, 0);
        private static readonly Vector3Int one = new(1, 1, 1);
        private static readonly Vector3Int unitX = new(1, 0, 0);
        private static readonly Vector3Int unitY = new(0, 1, 0);
        private static readonly Vector3Int unitZ = new(0, 0, 1);
        private static readonly Vector3Int up = new(0, 1, 0);
        private static readonly Vector3Int down = new(0, -1, 0);
        private static readonly Vector3Int right = new(1, 0, 0);
        private static readonly Vector3Int left = new(-1, 0, 0);
        private static readonly Vector3Int forward = new(0, 0, -1);
        private static readonly Vector3Int backward = new(0, 0, 1);

        #endregion

        #region Public Fields

        [DataMember] public int X;
        [DataMember] public int Y;
        [DataMember] public int Z;

        #endregion

        #region Public Properties

        public static Vector3Int Zero => zero;
        public static Vector3Int One => one;
        public static Vector3Int UnitX => unitX;
        public static Vector3Int UnitY => unitY;
        public static Vector3Int UnitZ => unitZ;
        public static Vector3Int Up => up;
        public static Vector3Int Down => down;
        public static Vector3Int Right => right;
        public static Vector3Int Left => left;
        public static Vector3Int Forward => forward;
        public static Vector3Int Backward => backward;

        #endregion

        #region Constructors

        public Vector3Int(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public Vector3Int(int value)
        {
            this.X = value;
            this.Y = value;
            this.Z = value;
        }
        public Vector3Int(Vector2Int value, int z)
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = z;
        }
        public Vector3Int(Vector2 value, int z)
        {
            this.X = (int)value.X;
            this.Y = (int)value.Y;
            this.Z = z;
        }

        #endregion

        #region Public Methods

        public static Vector3Int Add(Vector3Int value1, Vector3Int value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            value1.Z += value2.Z;

            return value1;
        }
        public static Vector3Int Subtract(Vector3Int value1, Vector3Int value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            return value1;
        }
        public static Vector3Int Multiply(Vector3Int value1, Vector3Int value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            value1.Z *= value2.Z;
            return value1;
        }
        public static Vector3Int Multiply(Vector3Int value1, int scaleFactor)
        {
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;
            value1.Z *= scaleFactor;
            return value1;
        }
        public static Vector3Int Divide(Vector3Int value1, Vector3Int value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            value1.Z /= value2.Z;
            return value1;
        }
        public static Vector3Int Divide(Vector3Int value1, int divider)
        {
            int factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            value1.Z *= factor;
            return value1;
        }

        public static Vector3Int CatmullRom(Vector3Int value1, Vector3Int value2, Vector3Int value3, Vector3Int value4, int amount)
        {
            return new Vector3Int(
                (int)MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
                (int)MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount),
                (int)MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount));
        }
        public void Ceiling()
        {
            this.X = (int)MathF.Ceiling(this.X);
            this.Y = (int)MathF.Ceiling(this.Y);
            this.Z = (int)MathF.Ceiling(this.Z);
        }
        public static Vector3Int Ceiling(Vector3Int value)
        {
            value.X = (int)MathF.Ceiling(value.X);
            value.Y = (int)MathF.Ceiling(value.Y);
            value.Z = (int)MathF.Ceiling(value.Z);
            return value;
        }
        public static Vector3Int Clamp(Vector3Int value1, Vector3Int min, Vector3Int max)
        {
            return new Vector3Int(
                MathHelper.Clamp(value1.X, min.X, max.X),
                MathHelper.Clamp(value1.Y, min.Y, max.Y),
                MathHelper.Clamp(value1.Z, min.Z, max.Z));
        }
        public static Vector3Int Cross(Vector3Int vector1, Vector3Int vector2)
        {
            return Cross(vector1, vector2);
        }
        public static float Distance(Vector3Int value1, Vector3Int value2)
        {
            return MathF.Sqrt(DistanceSquared(value1, value2));
        }
        public static float DistanceSquared(Vector3Int value1, Vector3Int value2)
        {
            return ((value1.X - value2.X) * (value1.X - value2.X)) +
                   ((value1.Y - value2.Y) * (value1.Y - value2.Y)) +
                   ((value1.Z - value2.Z) * (value1.Z - value2.Z));
        }

        public static int Dot(Vector3Int value1, Vector3Int value2)
        {
            return (value1.X * value2.X) + (value1.Y * value2.Y) + (value1.Z * value2.Z);
        }

        public static Vector3Int Max(Vector3Int value1, Vector3Int value2)
        {
            return new Vector3Int(
                MathHelper.Max(value1.X, value2.X),
                MathHelper.Max(value1.Y, value2.Y),
                MathHelper.Max(value1.Z, value2.Z));
        }
        public static Vector3Int Min(Vector3Int value1, Vector3Int value2)
        {
            return new Vector3Int(
                MathHelper.Min(value1.X, value2.X),
                MathHelper.Min(value1.Y, value2.Y),
                MathHelper.Min(value1.Z, value2.Z));
        }

        public static Vector3Int Negate(Vector3Int value)
        {
            value = new Vector3Int(-value.X, -value.Y, -value.Z);
            return value;
        }

        public readonly int Length()
        {
            return (int)MathF.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z));
        }
        public readonly int LengthSquared()
        {
            return (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z);
        }

        public readonly void Deconstruct(out int x, out int y, out int z)
        {
            x = this.X;
            y = this.Y;
            z = this.Z;
        }

        #endregion

        #region Operators

        // Implicit
        public static implicit operator Vector3Int(Vector3 value)
        {
            return new Vector3Int((int)value.X, (int)value.Y, (int)value.Z);
        }
        public static explicit operator Vector3(Vector3Int value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        // Equals
        public static bool operator ==(Vector3Int value1, Vector3Int value2)
        {
            return value1.X == value2.X
                && value1.Y == value2.Y
                && value1.Z == value2.Z;
        }
        public static bool operator !=(Vector3Int value1, Vector3Int value2)
        {
            return !(value1 == value2);
        }

        // Calc
        public static Vector3Int operator +(Vector3Int value1, Vector3Int value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            value1.Z += value2.Z;
            return value1;
        }
        public static Vector3Int operator -(Vector3Int value)
        {
            value = new Vector3Int(-value.X, -value.Y, -value.Z);
            return value;
        }
        public static Vector3Int operator -(Vector3Int value1, Vector3Int value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            return value1;
        }
        public static Vector3Int operator *(Vector3Int value1, Vector3Int value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            value1.Z *= value2.Z;
            return value1;
        }
        public static Vector3Int operator *(Vector3Int value, int scaleFactor)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            value.Z *= scaleFactor;
            return value;
        }
        public static Vector3Int operator *(int scaleFactor, Vector3Int value)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            value.Z *= scaleFactor;
            return value;
        }
        public static Vector3Int operator /(Vector3Int value1, Vector3Int value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            value1.Z /= value2.Z;
            return value1;
        }
        public static Vector3Int operator /(Vector3Int value1, int divider)
        {
            int factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            value1.Z *= factor;
            return value1;
        }

        #endregion

        #region Conversions

        public override readonly string ToString()
        {
            StringBuilder sb = new(32);
            _ = sb.Append("{X:");
            _ = sb.Append(this.X);
            _ = sb.Append(" Y:");
            _ = sb.Append(this.Y);
            _ = sb.Append(" Z:");
            _ = sb.Append(this.Z);
            _ = sb.Append('}');
            return sb.ToString();
        }

        #endregion

        #region Override
        public override readonly bool Equals(object obj)
        {
            if (obj is not Vector3Int)
            {
                return false;
            }

            Vector3Int other = (Vector3Int)obj;
            return this.X == other.X &&
                    this.Y == other.Y &&
                    this.Z == other.Z;
        }
        public readonly bool Equals(Vector3Int other)
        {
            return this.X == other.X &&
                    this.Y == other.Y &&
                    this.Z == other.Z;
        }
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(this.X, this.Y, this.Z);
        }
        #endregion
    }
}