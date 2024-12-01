using StardustSandbox.Core.Interfaces.General;

using System;

namespace StardustSandbox.Core.Entities
{
    public abstract class SEntityDescriptor
    {
        public Type AssociatedEntityType => this.associatedEntityType;

        protected Type associatedEntityType;

        public abstract SEntity CreateEntity(ISGame gameInstance);
    }
}