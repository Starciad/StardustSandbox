using StardustSandbox.Core.Entities;

using System.Collections.Generic;

namespace StardustSandbox.Core.Interfaces.Databases
{
    public interface ISEntityDatabase
    {
        IReadOnlyDictionary<string, SEntityDescriptor> RegisteredDescriptors { get; }

        void RegisterEntityDescriptor(SEntityDescriptor descriptor);
        SEntityDescriptor GetEntityDescriptor(string identifier);
    }
}
