using StardustSandbox.Actors;
using StardustSandbox.Actors.Common;
using StardustSandbox.Enums.Actors;
using StardustSandbox.Interfaces.Actors;
using StardustSandbox.Managers;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Databases
{
    internal static class ActorDatabase
    {
        private static IActorDescriptor[] descriptors;
        private static bool isLoaded = false;

        internal static void Load(ActorManager actorManager, World world)
        {
            if (isLoaded)
            {
                throw new InvalidOperationException($"{nameof(ActorDatabase)} has already been loaded.");
            }

            descriptors = [
                new ActorDescriptor<GulActor>(ActorIndex.Gul, () => new(ActorIndex.Gul, actorManager, world)),
            ];

            isLoaded = true;
        }

        internal static IActorDescriptor GetDescriptor(ActorIndex index)
        {
            return descriptors[(byte)index];
        }
    }
}
