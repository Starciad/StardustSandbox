using System;
using System.Diagnostics;
using System.Text;
using System.Runtime.Serialization;

using Microsoft.Xna.Framework;

namespace PixelDust.Core.Mathematics
{
    [DataContract]
    [DebuggerDisplay("{DebugDisplayString,nq}")]
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

        public static Vector3Int Zero
        {
            get { return zero; }
        }
        public static Vector3Int One
        {
            get { return one; }
        }
        public static Vector3Int UnitX
        {
            get { return unitX; }
        }
        public static Vector3Int UnitY
        {
            get { return unitY; }
        }
        public static Vector3Int UnitZ
        {
            get { return unitZ; }
        }
        public static Vector3Int Up
        {
            get { return up; }
        }
        public static Vector3Int Down
        {
            get { return down; }
        }
        public static Vector3Int Right
        {
            get { return right; }
        }
        public static Vector3Int Left
        {
            get { return left; }
        }
        public static Vector3Int Forward
        {
            get { return forward; }
        }
        public static Vector3Int Backward
        {
            get { return backward; }
        }

        #endregion

        #region Internal Properties

        internal readonly string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    X.ToString(), "  ",
                    Y.ToString(), "  ",
                    Z.ToString()
                );
            }
        }

        #endregion

        #region Constructors

        public Vector3Int(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3Int(int value)
        {
            X = value;
            Y = value;
            Z = value;
        }
        public Vector3Int(Vector2Int value, int z)
        {
            X = value.X;
            Y = value.Y;
            Z = z;
        }
        public Vector3Int(Vector2 value, int z)
        {
            X = (int)value.X;
            Y = (int)value.Y;
            Z = z;
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
            X = (int)MathF.Ceiling(X);
            Y = (int)MathF.Ceiling(Y);
            Z = (int)MathF.Ceiling(Z);
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
            return (value1.X - value2.X) * (value1.X - value2.X) +
                   (value1.Y - value2.Y) * (value1.Y - value2.Y) +
                   (value1.Z - value2.Z) * (value1.Z - value2.Z);
        }

        public static int Dot(Vector3Int value1, Vector3Int value2)
        {
            return value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z;
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
            return (int)MathF.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }
        public readonly int LengthSquared()
        {
            return (X * X) + (Y * Y) + (Z * Z);
        }

        public readonly void Deconstruct(out int x, out int y, out int z)
        {
            x = X;
            y = Y;
            z = Z;
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
            sb.Append("{X:");
            sb.Append(X);
            sb.Append(" Y:");
            sb.Append(Y);
            sb.Append(" Z:");
            sb.Append(Z);
            sb.Append('}');
            return sb.ToString();
        }

        #endregion

        #region Override
        public override readonly bool Equals(object obj)
        {
            if (obj is not Vector3Int)
                return false;

            Vector3Int other = (Vector3Int)obj;
            return X == other.X &&
                    Y == other.Y &&
                    Z == other.Z;
        }
        public readonly bool Equals(Vector3Int other)
        {
            return X == other.X &&
                    Y == other.Y &&
                    Z == other.Z;
        }
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
        #endregion
    }
}