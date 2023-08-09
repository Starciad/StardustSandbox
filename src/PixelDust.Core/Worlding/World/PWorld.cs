using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PixelDust.Core.Worlding
{
    public sealed partial class PWorld
    {
        public const int GridScale = 13;

        private readonly PWorldThreadingComponent _threading = new();
        private readonly PWorldChunkingComponent _chunking = new();

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
            _chunking.Initialize(this);
            _threading.Initialize(this);
        }

        public void Update()
        {
            if (States.IsPaused || States.IsUnloaded) return;
            _chunking.Update();
            _threading.Update();
        }

        public void Draw()
        {
            _chunking.Draw();
            _threading.Draw();
            DrawElements();
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
                        Slots[x, y].Color,
                        0f,
                        Vector2.Zero,
                        GridScale,
                        SpriteEffects.None,
                        0f
                    );
                }
            }
        }
    }
}