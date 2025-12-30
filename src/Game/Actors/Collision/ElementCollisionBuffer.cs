using System;

namespace StardustSandbox.Actors.Collision
{
    internal static class ElementCollisionBuffer
    {
        private const int CAPACITY = 256;

        private static readonly ElementCollisionInfo[] buffer = new ElementCollisionInfo[CAPACITY];
        private static int count = 0;

        internal static int Count => count;

        internal static void Clear()
        {
            count = 0;
        }

        internal static bool TryAdd(in ElementCollisionInfo info)
        {
            if (count >= CAPACITY)
            {
                return false;
            }

            buffer[count] = info;
            count++;
            return true;
        }

        internal static ArraySegment<ElementCollisionInfo> AsArraySegment()
        {
            return new ArraySegment<ElementCollisionInfo>(buffer, 0, count);
        }
    }
}
