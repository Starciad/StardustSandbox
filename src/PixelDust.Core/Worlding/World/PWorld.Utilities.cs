using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using PixelDust.Core.Engine;
using System;

namespace PixelDust.Core.Worlding
{
    public sealed partial class PWorld
    {
        public bool InsideTheWorldDimensions(Vector2 pos)
        {
            return (int)pos.X >= 0 && (int)pos.X < Infos.Width &&
                   (int)pos.Y >= 0 && (int)pos.Y < Infos.Height;
        }

        public void Restart()
        {
            Viewport viewport = PGraphics.Viewport;
            uint width = (uint)(viewport.Width / GridScale);
            uint height = (uint)(viewport.Height / GridScale);

            Clear();
            Slots = new PWorldSlot[width, height];

            Infos.SetWidth(width);
            Infos.SetHeight(height);
        }

        public void Clear()
        {
            if (Slots == null)
                return;

            for (int x = 0; x < Infos.Width; x++)
            {
                for (int y = 0; y < Infos.Height; y++)
                {
                    if (IsEmpty(new(x, y)))
                        continue;

                    TryDestroy(new(x, y));
                }
            }

            GC.Collect(GC.GetGeneration(Slots), GCCollectionMode.Forced);
        }

        public void Unload()
        {
            States.SetUnloaded(true);
            Clear();

            GC.Collect(GC.GetGeneration(Slots), GCCollectionMode.Forced);
        }

        public void Pause()
        {
            States.SetPaused(true);
        }

        public void Resume()
        {
            States.SetPaused(false);
        }
    }
}
