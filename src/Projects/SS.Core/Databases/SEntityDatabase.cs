using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed class SEntityDatabase(ISGame gameInstance) : SGameObject(gameInstance), ISEntityDatabase
    {
        public IReadOnlyDictionary<string, SEntityDescriptor> RegisteredDescriptors => this.registeredDescriptors;

        private readonly Dictionary<string, SEntityDescriptor> registeredDescriptors = [];

        public void RegisterEntityDescriptor(SEntityDescriptor descriptor)
        {
            this.registeredDescriptors.Add(descriptor.Identifier, descriptor);
        }

        public SEntityDescriptor GetEntityDescriptor(string entityIdentifier)
        {
            return this.registeredDescriptors[entityIdentifier];
        }
    }
}