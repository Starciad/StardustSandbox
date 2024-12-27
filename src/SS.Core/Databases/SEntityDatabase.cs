using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Objects;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed class SEntityDatabase(ISGame gameInstance) : SGameObject(gameInstance), ISEntityDatabase
    {
        public IReadOnlyDictionary<Type, SEntityDescriptor> RegisteredEntities => this._registeredEntities;

        private readonly Dictionary<Type, SEntityDescriptor> _registeredEntities = [];

        public void RegisterEntityDescriptor(SEntityDescriptor descriptor)
        {
            this._registeredEntities.Add(descriptor.AssociatedEntityType, descriptor);
        }

        public SEntityDescriptor GetEntityDescriptor<T>() where T : SEntity
        {
            return GetEntityDescriptor(typeof(T));
        }

        public SEntityDescriptor GetEntityDescriptor(Type entityType)
        {
            return this._registeredEntities[entityType];
        }
    }
}