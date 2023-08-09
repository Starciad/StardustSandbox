using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

using System;

namespace PixelDust.Core.Worlding
{
    public sealed partial class PWorld
    {
        public const int GridScale = 12;

        private readonly PWorldComponent[] _components = new PWorldComponent[]
        {
            new PWorldChunkingComponent(),
            new PWorldThreadingComponent(),
        };

        public PWorldStates States { get; private set; }
        public PWorldInfos Infos { get; private set; }

        internal PWorldSlot[,] Slots { get; private set; }
        internal PElementContext ElementContext { get; private set; }

        public PWorld()
        {
            States = new();
            Infos = new();

            ElementContext = new(this);
            Restart();
        }

        public void Initialize()
        {
            foreach (PWorldComponent component in _components)
            {
                component.Initialize(this);
            }
        }

        public void Update()
        {
            if (States.IsPaused || States.IsUnloaded) return;
            foreach (PWorldComponent component in _components)
            {
                component.Update();
            }
        }

        public void Draw()
        {
            DrawElements();
            foreach (PWorldComponent component in _components)
            {
                component.Draw();
            }
        }

        private void DrawElements()
        {
            for (int x = 0; x < Infos.Width; x++)
            {
                for (int y = 0; y < Infos.Height; y++)
                {
                    if (IsEmpty(new(x, y))) continue;

                    PGraphics.SpriteBatch.Draw(
                        PTextures.Pixel,
                        new Vector2(x * GridScale, y * GridScale),
                        null,
                        Slots[x, y].Infos.Color,
                        0f,
                        Vector2.Zero,
                        GridScale,
                        SpriteEffects.None,
                        0f
                    );
                }
            }
        }

        internal T GetComponent<T>() where T : PWorldComponent
        {
            return (T)Array.Find(_components, x => x.GetType() == typeof(T));
        }
    }
}