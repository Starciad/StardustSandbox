using StardustSandbox.Enums.Actors;
using StardustSandbox.Interfaces.Actors;

using System;

namespace StardustSandbox.Databases
{
    internal static class ActorDatabase
    {
        private static IActorDescriptor[] descriptors;
        private static bool isLoaded = false;

        internal static void Load()
        {
            if (isLoaded)
            {
                throw new InvalidOperationException($"{nameof(ActorDatabase)} has already been loaded.");
            }

            descriptors = [];

            isLoaded = true;
        }

        internal static IActorDescriptor GetDescriptor(ActorIndex index)
        {
            if (!isLoaded)
            {
                throw new InvalidOperationException($"{nameof(ActorDatabase)} has not been loaded.");
            }

            return descriptors[(byte)index];
        }
    }
}
