using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PixelDust.Core
{
    public sealed partial class World
    {
        private bool isUnloaded = false;

        public bool InsideTheWorldDimensions(Vector2 pos)
        {
            return (int)pos.X >= 0 && (int)pos.X < Width &&
                   (int)pos.Y >= 0 && (int)pos.Y < Height;
        }

        public void Restart()
        {
            Viewport viewport = PGraphics.Viewport;
            _slots = new WorldSlot[viewport.Width / GridScale, viewport.Height / GridScale];

            Width = (uint)_slots.GetLength(0);
            Height = (uint)_slots.GetLength(1);
        }

        public void Clear()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (IsEmpty(new(x, y)))
                        continue;

                    TryDestroy(new(x, y));
                }
            }
        }

        public void Unload()
        {
            isUnloaded = true;
            Clear();
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }
    }
}
