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
        public static Vector3Int Barycentric(Vector3Int value1, Vector3Int value2, Vector3Int value3, int amount1, int amount2)
        {
            return new Vector3Int(
                (int)MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
                (int)MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2),
                (int)MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2));
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
        public static int Distance(Vector3Int value1, Vector3Int value2)
        {
            return (int)MathF.Sqrt(DistanceSquared(value1, value2));
        }
        public static int DistanceSquared(Vector3Int value1, Vector3Int value2)
        {
            return (value1.X - value2.X) * (value1.X - value2.X) +
                   (value1.Y - value2.Y) * (value1.Y - value2.Y) +
                   (value1.Z - value2.Z) * (value1.Z - value2.Z);
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
        public static int Dot(Vector3Int value1, Vector3Int value2)
        {
            return value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z;
        }

        public readonly override bool Equals(object obj)
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
        public readonly override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public static Vector3Int Hermite(Vector3Int value1, Vector3Int tangent1, Vector3Int value2, Vector3Int tangent2, int amount)
        {
            return new Vector3Int((int)MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount),
                               (int)MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount),
                               (int)MathHelper.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount));
        }
        public readonly int Length()
        {
            return (int)MathF.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }
        public readonly int LengthSquared()
        {
            return (X * X) + (Y * Y) + (Z * Z);
        }
        public static Vector3Int Lerp(Vector3Int value1, Vector3Int value2, int amount)
        {
            return new Vector3Int(
                (int)MathHelper.Lerp(value1.X, value2.X, amount),
                (int)MathHelper.Lerp(value1.Y, value2.Y, amount),
                (int)MathHelper.Lerp(value1.Z, value2.Z, amount));
        }
        public static Vector3Int LerpPrecise(Vector3Int value1, Vector3Int value2, int amount)
        {
            return new Vector3Int(
                (int)MathHelper.LerpPrecise(value1.X, value2.X, amount),
                (int)MathHelper.LerpPrecise(value1.Y, value2.Y, amount),
                (int)MathHelper.LerpPrecise(value1.Z, value2.Z, amount));
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
        public static Vector3Int Negate(Vector3Int value)
        {
            value = new Vector3Int(-value.X, -value.Y, -value.Z);
            return value;
        }
        public void Normalize()
        {
            float factor = MathF.Sqrt((X * X) + (Y * Y) + (Z * Z));
            factor = 1f / factor;
            X *= (int)MathF.Round(factor);
            Y *= (int)MathF.Round(factor);
            Z *= (int)MathF.Round(factor);
        }
        public static Vector3Int Normalize(Vector3Int value)
        {
            float factor = MathF.Sqrt((value.X * value.X) + (value.Y * value.Y) + (value.Z * value.Z));
            factor = 1f / factor;
            int iFactor = (int)MathF.Round(factor);

            return new Vector3Int(value.X * iFactor, value.Y * iFactor, value.Z * iFactor);
        }
        public static Vector3Int Reflect(Vector3Int vector, Vector3Int normal)
        {
            // I is the original array
            // N is the normal of the incident plane
            // R = I - (2 * N * ( DotProduct[ I,N] ))
            Vector3Int reflectedVector;
            // inline the dotProduct here instead of calling method
            int dotProduct = ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);
            reflectedVector.X = (int)(vector.X - (2.0f * normal.X) * dotProduct);
            reflectedVector.Y = (int)(vector.Y - (2.0f * normal.Y) * dotProduct);
            reflectedVector.Z = (int)(vector.Z - (2.0f * normal.Z) * dotProduct);

            return reflectedVector;
        }
        public static Vector3Int SmoothStep(Vector3Int value1, Vector3Int value2, int amount)
        {
            return new Vector3Int(
                (int)MathHelper.SmoothStep(value1.X, value2.X, amount),
                (int)MathHelper.SmoothStep(value1.Y, value2.Y, amount),
                (int)MathHelper.SmoothStep(value1.Z, value2.Z, amount));
        }
        public static Vector3Int Subtract(Vector3Int value1, Vector3Int value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            return value1;
        }

        public readonly void Deconstruct(out int x, out int y, out int z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        #endregion

        #region Operators

        public static implicit operator Vector3Int(System.Numerics.Vector3 value)
        {
            return new Vector3Int((int)value.X, (int)value.Y, (int)value.Z);
        }
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

        public readonly System.Numerics.Vector3 ToNumerics()
        {
            return new System.Numerics.Vector3(X, Y, Z);
        }
        public readonly override string ToString()
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
    }
}