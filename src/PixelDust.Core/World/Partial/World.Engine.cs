using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Core
{
    public sealed partial class World
    {
        private readonly List<Vector2> _tempSlots = new();
        // Chunks

        #region Initialization
        public void Initialize()
        {

        }
        #endregion

        #region Update
        public void Update()
        {
            if (IsPaused || isUnloaded) return;

            uint tempTotalElements = 0;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (_slots[x, y].IsEmpty()) continue;

                    _tempSlots.Add(new(x, y));
                    tempTotalElements++;
                }
            }

            for (int i = 0; i < tempTotalElements; i++)
            {
                Vector2 pos = _tempSlots[i];

                _EContext.Update(_slots[(int)pos.X, (int)pos.Y], pos);
                if (!TryGetElement(pos, out PElement value)) continue;

                value?.Update(_EContext);
            }

            TotalElements = tempTotalElements;
            _tempSlots.Clear();
        }
        #endregion

        #region Draw
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
        #endregion
    }
}
