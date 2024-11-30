using StardustSandbox.Core.Interfaces.General;

using System;

namespace StardustSandbox.Core.Entities
{
    public abstract class SEntityDescriptor
    {
        public uint Id => this.id;
        public Type AssociatedEntityType => this.associatedEntityType;

        protected uint id;
        protected Type associatedEntityType;

        public abstract SEntity CreateEntity(ISGame gameInstance);
    }
}