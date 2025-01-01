using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Components;
using StardustSandbox.Core.Components.Common.World;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements.Contexts;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.IO.Files.World;
using StardustSandbox.Core.Objects;
using StardustSandbox.Core.World.General;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld : SGameObject, ISWorld
    {
        public SWorldInfo Infos { get; }
        public SWorldTime Time { get; }

        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }
        public int ActiveEntitiesCount => this.instantiatedEntities.Count;

        // System
        private SWorldSaveFile currentlySelectedWorldSaveFile;

        // General
        private SWorldSlot[,] slots;
        private uint currentFrameUpdateSlotsDelay;

        // World
        private readonly uint totalFramesUpdateSlotsDelay = 5;
        private readonly SComponentContainer componentContainer;
        private readonly SWorldChunkingComponent worldChunkingComponent;
        private readonly SElementContext worldElementContext;

        // Entities
        private readonly List<SEntity> instantiatedEntities = new(SEntityConstants.ACTIVE_ENTITIES_LIMIT);

        // Pools
        private readonly SObjectPool worldSlotsPool;
        private readonly Dictionary<string, SObjectPool> entityPools = [];

        public SWorld(ISGame gameInstance) : base(gameInstance)
        {
            this.Infos = new();
            this.Time = new(gameInstance);

            this.worldSlotsPool = new();
            this.componentContainer = new(gameInstance);

            this.worldChunkingComponent = this.componentContainer.AddComponent(new SWorldChunkingComponent(gameInstance, this));
            _ = this.componentContainer.AddComponent(new SWorldUpdatingComponent(gameInstance, this));
            _ = this.componentContainer.AddComponent(new SWorldRenderingComponent(gameInstance, this));

            this.worldElementContext = new(this);
        }

        public void Reset()
        {
            this.currentlySelectedWorldSaveFile = null;

            this.Infos.Reset();
            this.Time.Reset();

            this.componentContainer.Reset();
            Clear();
        }
    }
}