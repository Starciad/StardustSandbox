using PixelDust.Core.Elements.Context;
using PixelDust.Core.Utilities;
using PixelDust.Core.Worlding.Components;
using PixelDust.Core.Worlding.Components.Chunking;
using PixelDust.Core.Worlding.Components.Threading;
using PixelDust.Core.Worlding.World.Data;
using PixelDust.Core.Worlding.World.Slots;
using PixelDust.Mathematics;

using System;

namespace PixelDust.Core.Worlding
{
    public sealed partial class PWorld
    {
        public const int Scale = 32;
        public const float Gravity = 9.81f;

        public PWorldStates States { get; private set; } = new();
        public PWorldInfos Infos { get; private set; } = new();

        internal PWorldElementSlot[,] Elements { get; private set; }

        private readonly PWorldComponent[] _components =
        [
            new PWorldChunkingComponent(),
            new PWorldThreadingComponent(),
        ];

        internal readonly PElementContext elementUpdateContext;
        internal readonly PElementContext elementDrawContext;

        private readonly PTimer updateTimer = new(0.35f);

        public PWorld()
        {
            this.elementUpdateContext = new(this);
            this.elementDrawContext = new(this);
        }

        // Engine
        public void Initialize()
        {
            Restart();

            foreach (PWorldComponent component in this._components)
            {
                component.Initialize(this);
            }

            this.States.IsActive = true;
        }
        public void Update()
        {
            // Check current game status
            if (!this.States.IsActive || this.States.IsPaused)
            {
                return;
            }

            // Check update delay for the current game world
            this.updateTimer.Update();
            if (this.updateTimer.IsFinished)
            {
                this.updateTimer.Restart();
            }
            else
            {
                return;
            }

            // Update world
            foreach (PWorldComponent component in this._components)
            {
                component.Update();
            }
        }

        // Components
        internal T GetComponent<T>() where T : PWorldComponent
        {
            return (T)Array.Find(this._components, x => x.GetType() == typeof(T));
        }

        // World
        public bool InsideTheWorldDimensions(Vector2Int pos)
        {
            return pos.X >= 0 && pos.X < this.Infos.Size.Width &&
                   pos.Y >= 0 && pos.Y < this.Infos.Size.Height;
        }

        // States
        public void Restart()
        {
            this.Elements = new PWorldElementSlot[this.Infos.Size.Width, this.Infos.Size.Height];
            this.updateTimer.Restart();
        }
        public void Pause()
        {
            this.States.IsPaused = true;
        }
        public void Resume()
        {
            this.States.IsPaused = false;
        }

        // Tools
        public void Clear()
        {
            if (this.Elements == null)
            {
                return;
            }

            for (int x = 0; x < this.Infos.Size.Width; x++)
            {
                for (int y = 0; y < this.Infos.Size.Height; y++)
                {
                    if (IsEmptyElementSlot(new(x, y)))
                    {
                        continue;
                    }

                    _ = TryDestroyElement(new(x, y));
                }
            }
        }
    }
}