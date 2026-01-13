/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

using System;

namespace StardustSandbox.Randomness
{
    /// <summary>
    /// Provides static methods for generating random values of various numeric types and evaluating probabilistic conditions.
    /// </summary>
    internal static class SSRandom
    {
        /// <summary>
        /// The underlying <see cref="Random"/> instance used for random number generation.
        /// </summary>
        private static readonly Random random = new();

        /// <summary>
        /// Returns a random <see cref="byte"/> value within the specified inclusive range.
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="byte"/> between <paramref name="min"/> and <paramref name="max"/>, inclusive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        internal static byte Range(byte min, byte max)
        {
            return (byte)random.Next(min, max + 1);
        }

        /// <summary>
        /// Returns a random <see cref="sbyte"/> value within the specified inclusive range.
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="sbyte"/> between <paramref name="min"/> and <paramref name="max"/>, inclusive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        internal static sbyte Range(sbyte min, sbyte max)
        {
            return (sbyte)random.Next(min, max + 1);
        }

        /// <summary>
        /// Returns a random <see cref="short"/> value within the specified inclusive range.
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="short"/> between <paramref name="min"/> and <paramref name="max"/>, inclusive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        internal static short Range(short min, short max)
        {
            return (short)random.Next(min, max + 1);
        }

        /// <summary>
        /// Returns a random <see cref="ushort"/> value within the specified inclusive range.
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="ushort"/> between <paramref name="min"/> and <paramref name="max"/>, inclusive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        internal static ushort Range(ushort min, ushort max)
        {
            return (ushort)random.Next(min, max + 1);
        }

        /// <summary>
        /// Returns a random <see cref="int"/> value within the specified inclusive range.
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="int"/> between <paramref name="min"/> and <paramref name="max"/>, inclusive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        internal static int Range(int min, int max)
        {
            return random.Next(min, max + 1);
        }

        /// <summary>
        /// Returns a random <see cref="uint"/> value within the specified inclusive range.
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="uint"/> between <paramref name="min"/> and <paramref name="max"/>, inclusive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="min"/> is greater than <paramref name="max"/> or if values exceed <see cref="int.MaxValue"/>.</exception>
        internal static uint Range(uint min, uint max)
        {
            return (uint)random.Next((int)min, (int)(max + 1));
        }

        /// <summary>
        /// Returns a random <see cref="long"/> value within the specified inclusive range.
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="long"/> between <paramref name="min"/> and <paramref name="max"/>, inclusive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        internal static long Range(long min, long max)
        {
            return min + (long)(random.NextDouble() * (max - min + 1));
        }

        /// <summary>
        /// Returns a random <see cref="ulong"/> value within the specified inclusive range.
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="ulong"/> between <paramref name="min"/> and <paramref name="max"/>, inclusive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="min"/> is greater than <paramref name="max"/> or if values exceed <see cref="long.MaxValue"/>.</exception>
        internal static ulong Range(ulong min, ulong max)
        {
            return min + (ulong)(random.NextDouble() * (long)(max - min + 1));
        }

        /// <summary>
        /// Returns a random <see cref="float"/> value within the specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The exclusive upper bound.</param>
        /// <returns>A random <see cref="float"/> greater than or equal to <paramref name="min"/> and less than <paramref name="max"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        internal static float Range(float min, float max)
        {
            return Convert.ToSingle((random.NextDouble() * (max - min)) + min);
        }

        /// <summary>
        /// Returns a random <see cref="double"/> value within the specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The exclusive upper bound.</param>
        /// <returns>A random <see cref="double"/> greater than or equal to <paramref name="min"/> and less than <paramref name="max"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        internal static double Range(double min, double max)
        {
            return (double)((random.NextDouble() * (max - min)) + min);
        }

        /// <summary>
        /// Returns a random <see cref="decimal"/> value within the specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The exclusive upper bound.</param>
        /// <returns>A random <see cref="decimal"/> greater than or equal to <paramref name="min"/> and less than <paramref name="max"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        internal static decimal Range(decimal min, decimal max)
        {
            return ((decimal)random.NextDouble() * (max - min)) + min;
        }

        /// <summary>
        /// Returns a random <see cref="byte"/> value between 0 and the specified maximum value, inclusive.
        /// </summary>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="byte"/> between 0 and <paramref name="max"/>, inclusive.</returns>
        internal static byte Range(byte max)
        {
            return Range((byte)0, max);
        }

        /// <summary>
        /// Returns a random <see cref="sbyte"/> value between 0 and the specified maximum value, inclusive.
        /// </summary>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="sbyte"/> between 0 and <paramref name="max"/>, inclusive.</returns>
        internal static sbyte Range(sbyte max)
        {
            return Range((sbyte)0, max);
        }

        /// <summary>
        /// Returns a random <see cref="short"/> value between 0 and the specified maximum value, inclusive.
        /// </summary>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="short"/> between 0 and <paramref name="max"/>, inclusive.</returns>
        internal static short Range(short max)
        {
            return Range((short)0, max);
        }

        /// <summary>
        /// Returns a random <see cref="ushort"/> value between 0 and the specified maximum value, inclusive.
        /// </summary>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="ushort"/> between 0 and <paramref name="max"/>, inclusive.</returns>
        internal static ushort Range(ushort max)
        {
            return Range((ushort)0, max);
        }

        /// <summary>
        /// Returns a random <see cref="int"/> value between 0 and the specified maximum value, inclusive.
        /// </summary>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="int"/> between 0 and <paramref name="max"/>, inclusive.</returns>
        internal static int Range(int max)
        {
            return Range(0, max);
        }

        /// <summary>
        /// Returns a random <see cref="uint"/> value between 0 and the specified maximum value, inclusive.
        /// </summary>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="uint"/> between 0 and <paramref name="max"/>, inclusive.</returns>
        internal static uint Range(uint max)
        {
            return Range(0u, max);
        }

        /// <summary>
        /// Returns a random <see cref="long"/> value between 0 and the specified maximum value, inclusive.
        /// </summary>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="long"/> between 0 and <paramref name="max"/>, inclusive.</returns>
        internal static long Range(long max)
        {
            return Range(0L, max);
        }

        /// <summary>
        /// Returns a random <see cref="ulong"/> value between 0 and the specified maximum value, inclusive.
        /// </summary>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns>A random <see cref="ulong"/> between 0 and <paramref name="max"/>, inclusive.</returns>
        internal static ulong Range(ulong max)
        {
            return Range(0UL, max);
        }

        /// <summary>
        /// Returns a random <see cref="float"/> value between 0 (inclusive) and the specified maximum value (exclusive).
        /// </summary>
        /// <param name="max">The exclusive upper bound.</param>
        /// <returns>A random <see cref="float"/> greater than or equal to 0 and less than <paramref name="max"/>.</returns>
        internal static float Range(float max)
        {
            return Range(0f, max);
        }

        /// <summary>
        /// Returns a random <see cref="double"/> value between 0 (inclusive) and the specified maximum value (exclusive).
        /// </summary>
        /// <param name="max">The exclusive upper bound.</param>
        /// <returns>A random <see cref="double"/> greater than or equal to 0 and less than <paramref name="max"/>.</returns>
        internal static double Range(double max)
        {
            return Range(0.0, max);
        }

        /// <summary>
        /// Returns a random <see cref="decimal"/> value between 0 (inclusive) and the specified maximum value (exclusive).
        /// </summary>
        /// <param name="max">The exclusive upper bound.</param>
        /// <returns>A random <see cref="decimal"/> greater than or equal to 0 and less than <paramref name="max"/>.</returns>
        internal static decimal Range(decimal max)
        {
            return Range(0m, max);
        }

        /// <summary>
        /// Returns a random boolean value.
        /// </summary>
        /// <returns><c>true</c> or <c>false</c> with equal probability.</returns>
        internal static bool GetBool()
        {
            return random.Next(2) == 0;
        }

        /// <summary>
        /// Evaluates a probabilistic event with a specified chance out of 100.
        /// </summary>
        /// <param name="chance">The number of chances for the event to occur (0-100).</param>
        /// <returns><c>true</c> if the event occurs; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method is equivalent to calling <see cref="Chance(int, int)"/> with <paramref name="total"/> set to 100.
        /// </remarks>
        internal static bool Chance(int chance)
        {
            return Chance(chance, 100);
        }

        /// <summary>
        /// Evaluates a probabilistic event with a specified chance out of a total.
        /// </summary>
        /// <param name="chance">The number of chances for the event to occur.</param>
        /// <param name="total">The total number of possible outcomes.</param>
        /// <returns><c>true</c> if the event occurs; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="chance"/> is negative or greater than <paramref name="total"/>.</exception>
        /// <example>
        /// <code>
        /// // 25% chance to return true
        /// bool result = RFRandom.Chance(25, 100);
        /// </code>
        /// </example>
        internal static bool Chance(int chance, int total)
        {
            return Range(0, total) < chance;
        }

        /// <summary>
        /// Evaluates a probabilistic event with a specified chance out of 100 using float precision.
        /// </summary>
        /// <param name="chance">The number of chances for the event to occur (0.0-100.0).</param>
        /// <returns><c>true</c> if the event occurs; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method is equivalent to calling <see cref="Chance(float, float)"/> with <paramref name="total"/> set to 100.0.
        /// </remarks>
        internal static bool Chance(double chance)
        {
            return Chance(chance, 100.0);
        }

        /// <summary>
        /// Evaluates a probabilistic event with a specified chance out of a total using float precision.
        /// </summary>
        /// <param name="chance">The number of chances for the event to occur.</param>
        /// <param name="total">The total number of possible outcomes.</param>
        /// <returns><c>true</c> if the event occurs; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="chance"/> is negative or greater than <paramref name="total"/>.</exception>
        /// <example>
        /// <code>
        /// // 25% chance to return true
        /// bool result = SSRandom.Chance(25.0, 100.0);
        /// </code>
        /// </example>
        internal static bool Chance(double chance, double total)
        {
            return chance < 0.0 || chance > total
                ? throw new ArgumentOutOfRangeException(nameof(chance), "Chance must be between 0 and total (inclusive).")
                : Range(0.0, total) < chance;
        }

        internal static double GetDouble()
        {
            return (double)random.NextDouble();
        }

        internal static float GetFloat()
        {
            return Convert.ToSingle(random.NextDouble());
        }
    }
}

