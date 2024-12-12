using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.Managers;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Managers
{
    internal sealed class SEntityManager(ISGame gameInstance) : SManager(gameInstance), ISEntityManager
    {
        public SEntity[] InstantiatedEntities => [.. this.instantiatedEntities];

        private readonly List<SEntity> instantiatedEntities = [];
        private readonly Dictionary<Type, SObjectPool> entityPools = [];

        public override void Initialize()
        {
            foreach (Type entityType in this.SGameInstance.EntityDatabase.RegisteredEntities.Keys)
            {
                this.entityPools.Add(entityType, new());
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.InstantiatedEntities.Length; i++)
            {
                this.InstantiatedEntities[i]?.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.InstantiatedEntities.Length; i++)
            {
                this.InstantiatedEntities[i]?.Draw(gameTime, spriteBatch);
            }
        }

        public SEntity Instantiate(SEntityDescriptor entityDescriptor, Action<SEntity> entityConfigurationAction)
        {
            SEntity entity = this.entityPools[entityDescriptor.AssociatedEntityType].TryGet(out ISPoolableObject pooledObject) ? (SEntity)pooledObject : this.SGameInstance.EntityDatabase.GetEntityDescriptor(entityDescriptor.AssociatedEntityType).CreateEntity(this.SGameInstance);

            this.instantiatedEntities.Add(entity);

            entityConfigurationAction?.Invoke(entity);
            entity.Initialize();
            return entity;
        }

        public void Remove(SEntity entity)
        {
            this.entityPools[entity.GetType()].Add(entity);
            _ = this.instantiatedEntities.Remove(entity);
        }

        public void RemoveAll()
        {
            foreach (SEntity entity in this.InstantiatedEntities)
            {
                Remove(entity);
            }
        }

        public void Destroy(SEntity entity)
        {
            this.entityPools[entity.GetType()].Add(entity);
            _ = this.instantiatedEntities.Remove(entity);
            entity.Destroy();
        }

        public void DestroyAll()
        {
            foreach (SEntity entity in this.InstantiatedEntities)
            {
                Destroy(entity);
            }
        }

        public void Reset()
        {
            foreach (SEntity entity in this.InstantiatedEntities)
            {
                entity.Reset();
            }
        }
    }
}