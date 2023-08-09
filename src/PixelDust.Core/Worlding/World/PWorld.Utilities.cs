using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using PixelDust.Core.Engine;
using System;

namespace PixelDust.Core.Worlding
{
    public static partial class PWorld
    {
        public static bool InsideTheWorldDimensions(Vector2 pos)
        {
            return (int)pos.X >= 0 && (int)pos.X < Infos.Width &&
                   (int)pos.Y >= 0 && (int)pos.Y < Infos.Height;
        }

        public static void Restart()
        {
            Viewport viewport = PGraphics.Viewport;
            uint width = (uint)(viewport.Width / GridScale);
            uint height = (uint)(viewport.Height / GridScale);

            Clear();
            Slots = new PWorldSlot[width, height];

            Infos.SetWidth(width);
            Infos.SetHeight(height);
        }

        public static void Clear()
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

        public static void Unload()
        {
            States.SetUnloaded(true);
            Clear();

            GC.Collect(GC.GetGeneration(Slots), GCCollectionMode.Forced);
        }

        public static void Pause()
        {
            States.SetPaused(true);
        }

        public static void Resume()
        {
            States.SetPaused(false);
        }
    }
}
