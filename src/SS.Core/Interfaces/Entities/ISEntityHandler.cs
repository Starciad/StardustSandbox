using StardustSandbox.Core.Entities;
using System;

namespace StardustSandbox.Core.Interfaces.Entities
{
    public interface ISEntityHandler
    {
        int ActiveEntitiesCount { get; }

        SEntity InstantiateEntity(string entityIdentifier, Action<SEntity> entityConfigurationAction);
        bool TryInstantiateEntity(string entityIdentifier, Action<SEntity> entityConfigurationAction, out SEntity entity);

        void RemoveEntity(SEntity entity);
        bool TryRemoveEntity(SEntity entity);

        void DestroyEntity(SEntity entity);
        bool TryDestroyEntity(SEntity entity);

        void RemoveAllEntity();
        void DestroyAllEntity();
    }
}
