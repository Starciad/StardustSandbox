using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Components;
using StardustSandbox.Core.Components.Common.World;
using StardustSandbox.Core.Elements.Contexts;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.IO.Files.Saving;
using StardustSandbox.Core.Objects;
using StardustSandbox.Core.World.General;
using StardustSandbox.Core.World.Slots;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld : SGameObject, ISWorld
    {
        public SWorldInfo Infos { get; }
        public SWorldTime Time { get; }
        public SWorldSimulation Simulation { get; }

        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }

        // System
        private SSaveFile currentlySelectedWorldSaveFile;

        // General
        private SWorldSlot[,] slots;
        
        // World
        private readonly SComponentContainer componentContainer;
        private readonly SWorldChunkingComponent worldChunkingComponent;
        private readonly SElementContext worldElementContext;

        // Pools
        private readonly SObjectPool worldSlotsPool;

        public SWorld(ISGame gameInstance) : base(gameInstance)
        {
            this.Infos = new();
            this.Time = new(gameInstance);
            this.Simulation = new();

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

            this.componentContainer.Reset();
            Clear();
        }
    }
}