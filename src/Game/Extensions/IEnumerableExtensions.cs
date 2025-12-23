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
