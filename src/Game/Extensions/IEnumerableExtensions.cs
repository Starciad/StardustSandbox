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

using StardustSandbox.Randomness;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Extensions
{
    internal static class IEnumerableExtensions
    {
        internal static T GetRandomItem<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentException("The collection cannot be null.");
            }

            using IEnumerator<T> enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                throw new ArgumentException("The collection cannot be empty.");
            }

            T current = enumerator.Current;
            int index = 1;

            while (enumerator.MoveNext())
            {
                if (SSRandom.Range(index + 1) == 0)
                {
                    current = enumerator.Current;
                }

                index++;
            }

            return current;
        }

        internal static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentException("The collection cannot be null.");
            }

            List<T> list = [.. source];
            int count = list.Count;

            if (count <= 1)
            {
                return list;
            }

            for (int i = 0; i < count; i++)
            {
                int j = SSRandom.Range(i, count - 1);
                (list[j], list[i]) = (list[i], list[j]);
            }

            return list;
        }
    }
}

