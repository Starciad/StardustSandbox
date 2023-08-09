using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

using System;

namespace PixelDust.Core.Worlding
{
    public static partial class PWorld
    {
        public const int GridScale = 12;

        private static readonly PWorldComponent[] _components = new PWorldComponent[]
        {
            new PWorldChunkingComponent(),
            new PWorldThreadingComponent(),
        };

        public static PWorldStates States { get; private set; } = new();
        public static PWorldInfos Infos { get; private set; } = new();

        internal static PWorldSlot[,] Slots { get; private set; }
        internal static PElementContext ElementContext { get; private set; }

        internal static void Initialize()
        {
            ElementContext = new();
            Restart();

            foreach (PWorldComponent component in _components)
            {
                component.Initialize();
            }
        }

        internal static void Update()
        {
            if (States.IsPaused || States.IsUnloaded) return;
            foreach (PWorldComponent component in _components)
            {
                component.Update();
            }
        }

        internal static void Draw()
        {
            DrawElements();
            foreach (PWorldComponent component in _components)
            {
                component.Draw();
            }
        }

        private static void DrawElements()
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

        internal static T GetComponent<T>() where T : PWorldComponent
        {
            return (T)Array.Find(_components, x => x.GetType() == typeof(T));
        }
    }
}