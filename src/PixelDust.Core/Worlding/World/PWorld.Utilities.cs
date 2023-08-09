using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using PixelDust.Core.Engine;

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

            Slots = new PWorldSlot[width, height];

            Infos.SetWidth(width);
            Infos.SetHeight(height);
        }

        public void Clear()
        {
            for (int x = 0; x < Infos.Width; x++)
            {
                for (int y = 0; y < Infos.Height; y++)
                {
                    if (IsEmpty(new(x, y)))
                        continue;

                    TryDestroy(new(x, y));
                }
            }
        }

        public void Unload()
        {
            States.SetUnloaded(true);
            Clear();
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
