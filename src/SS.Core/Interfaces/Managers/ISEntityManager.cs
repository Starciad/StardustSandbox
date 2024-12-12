using StardustSandbox.Core.Entities;

using System;

namespace StardustSandbox.Core.Interfaces.Managers
{
    public interface ISEntityManager : ISManager
    {
        SEntity[] InstantiatedEntities { get; }

        SEntity Instantiate(SEntityDescriptor entityDescriptor, Action<SEntity> entityConfigurationAction);

        void Remove(SEntity entity);
        void RemoveAll();

        void Destroy(SEntity entity);
        void DestroyAll();
    }
}
