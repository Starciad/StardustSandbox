using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces.Collections;

using System;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld
    {
        public SEntity InstantiateEntity(string entityIdentifier, Action<SEntity> entityConfigurationAction)
        {
            _ = TryInstantiateEntity(entityIdentifier, entityConfigurationAction, out SEntity entity);

            return entity;
        }

        public bool TryInstantiateEntity(string entityIdentifier, Action<SEntity> entityConfigurationAction, out SEntity entity)
        {
            if (this.instantiatedEntities.Count >= SEntityConstants.ACTIVE_ENTITIES_LIMIT)
            {
                entity = null;
                return false;
            }

            if (!this.entityPools.TryGetValue(entityIdentifier, out SObjectPool objectPool))
            {
                objectPool = new();
                this.entityPools[entityIdentifier] = objectPool;
            }

            if (!objectPool.TryGet(out ISPoolableObject value))
            {
                SEntityDescriptor entityDescriptor = this.SGameInstance.EntityDatabase.GetEntityDescriptor(entityIdentifier);
                value = entityDescriptor.CreateEntity(this.SGameInstance);
            }

            entity = value as SEntity;

            if (entity == null)
            {
                return false;
            }

            this.instantiatedEntities.Add(entity);

            entityConfigurationAction?.Invoke(entity);
            entity.Initialize();

            return true;
        }

        public void RemoveEntity(SEntity entity)
        {
            _ = this.instantiatedEntities.Remove(entity);
            this.entityPools[entity.Identifier].Add(entity);
        }

        public void DestroyEntity(SEntity entity)
        {
            RemoveEntity(entity);
            entity.Destroy();
        }

        public void RemoveAllEntity()
        {
            for (int i = 0; i < this.ActiveEntitiesCount; i++)
            {
                SEntity entity = this.instantiatedEntities[i];

                if (entity == null)
                {
                    continue;
                }

                RemoveEntity(entity);
            }
        }

        public void DestroyAllEntity()
        {
            for (int i = 0; i < this.ActiveEntitiesCount; i++)
            {
                SEntity entity = this.instantiatedEntities[i];

                if (entity == null)
                {
                    continue;
                }

                DestroyEntity(entity);
            }
        }
    }
}
