using StardustSandbox.Core.Entities;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Interfaces.Databases
{
    public interface ISEntityDatabase
    {
        IReadOnlyDictionary<Type, SEntityDescriptor> RegisteredEntities { get; }

        void RegisterEntityDescriptor(SEntityDescriptor descriptor);
        SEntityDescriptor GetEntityDescriptor<T>() where T : SEntity;
        SEntityDescriptor GetEntityDescriptor(Type entityType);
    }
}
