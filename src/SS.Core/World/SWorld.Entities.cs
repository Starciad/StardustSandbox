﻿using StardustSandbox.Core.Collections;
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
            _ = TryRemoveEntity(entity);
        }

        public bool TryRemoveEntity(SEntity entity)
        {
            if (this.instantiatedEntities.Contains(entity) && this.entityPools.TryGetValue(entity.Identifier, out SObjectPool objectPool))
            {
                objectPool.Add(entity);
                return this.instantiatedEntities.Remove(entity);
            }

            return false;
        }

        public void DestroyEntity(SEntity entity)
        {
            RemoveEntity(entity);
            entity.Destroy();
        }

        public bool TryDestroyEntity(SEntity entity)
        {
            if (TryRemoveEntity(entity))
            {
                entity.Destroy();
                return true;
            }

            return false;
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
