using PixelDust.Core.Elements;

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

        private readonly PWorldComponent[] _components = new PWorldComponent[]
        {
            new PWorldChunkingComponent(),
            new PWorldThreadingComponent(),
        };

        internal readonly PElementContext elementUpdateContext;
        internal readonly PElementContext elementDrawContext;

        public PWorld()
        {
            elementUpdateContext = new(this);
            elementDrawContext = new(this);
        }

        // Engine
        public void Initialize()
        {
            Restart();

            foreach (PWorldComponent component in _components)
                component.Initialize(this);

            States.IsActive = true;
        }
        public void Update()
        {
            if (!States.IsActive || States.IsPaused) return;
            foreach (PWorldComponent component in _components)
            {
                component.Update();
            }
        }

        // Components
        internal T GetComponent<T>() where T : PWorldComponent
        {
            return (T)Array.Find(_components, x => x.GetType() == typeof(T));
        }

        // World
        public bool InsideTheWorldDimensions(Vector2Int pos)
        {
            return pos.X >= 0 && pos.X < Infos.Size.Width &&
                   pos.Y >= 0 && pos.Y < Infos.Size.Height;
        }

        // States
        public void Restart()
        {
            Elements = new PWorldElementSlot[Infos.Size.Width, Infos.Size.Height];
        }
        public void Pause()
        {
            States.IsPaused = true;
        }
        public void Resume()
        {
            States.IsPaused = false;
        }

        // Tools
        public void Clear()
        {
            if (Elements == null)
                return;

            for (int x = 0; x < Infos.Size.Width; x++)
            {
                for (int y = 0; y < Infos.Size.Height; y++)
                {
                    if (IsEmptyElementSlot(new(x, y)))
                        continue;

                    TryDestroyElement(new(x, y));
                }
            }
        }
    }
}