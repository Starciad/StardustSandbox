using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PixelDust.Core
{
    public sealed partial class World
    {
        public void Initialize()
        {
            BuildWorldThreads();
        }

        public void Update()
        {
            if (IsPaused || isUnloaded) return;
            UpdateWorldThreads();
        }

        public void Draw()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (IsEmpty(new(x, y))) continue;

                    PGraphics.SpriteBatch.Draw(
                        PTextures.Pixel,
                        new Vector2(x * GridScale, y * GridScale),
                        null,
                        _slots[x, y].TargetColor,
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