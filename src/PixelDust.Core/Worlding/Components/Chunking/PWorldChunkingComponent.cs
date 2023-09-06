#pragma warning disable IDE0051

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Engine;
using PixelDust.Core.Mathematics;

namespace PixelDust.Core.Worlding
{
    internal sealed class PWorldChunkingComponent : PWorldComponent
    {
        internal static short DefaultChunkSize => 6;

        private int worldChunkWidth;
        private int worldChunkHeight;

        private PWorldChunk[,] _chunks;

        protected override void OnInitialize()
        {
            _chunks = new PWorldChunk[(WorldInstance.Infos.Size.Width / DefaultChunkSize) + 1,
                                      (WorldInstance.Infos.Size.Height / DefaultChunkSize) + 1];

            worldChunkWidth = _chunks.GetLength(0);
            worldChunkHeight = _chunks.GetLength(1);

            for (int x = 0; x < worldChunkWidth; x++)
            {
                for (int y = 0; y < worldChunkHeight; y++)
                {
                    _chunks[x, y] = new(new(x * DefaultChunkSize * PWorld.Scale, y * DefaultChunkSize * PWorld.Scale), DefaultChunkSize);
                }
            }
        }

        protected override void OnUpdate()
        {
            for (int x = 0; x < worldChunkWidth; x++)
            {
                for (int y = 0; y < worldChunkHeight; y++)
                {
                    _chunks[x, y].Update();
                }
            }
        }
        protected override void OnDraw()
        {
#if DEBUG
            // Debug methods
            DEBUG_DrawActiveChunks();
#endif
        }

        internal bool TryGetChunkUpdateState(Vector2Int pos, out bool result)
        {
            result = false;
            Vector2Int targetPos = ToChunkCoordinateSystem(pos);

            if (!IsWithinChunkBoundaries(targetPos))
                return false;

            result = _chunks[targetPos.X, targetPos.Y].ShouldUpdate;
            return true;
        }
        internal int GetActiveChunksCount()
        {
            int result = 0;
            for (int x = 0; x < worldChunkWidth; x++)
            {
                for (int y = 0; y < worldChunkHeight; y++)
                {
                    if (_chunks[x, y].ShouldUpdate)
                        result++;
                }
            }

            return result;
        }
        
        internal bool TryNotifyChunk(Vector2Int pos)
        {
            Vector2Int targetPos = ToChunkCoordinateSystem(pos);

            if (IsWithinChunkBoundaries(targetPos))
            {
                _chunks[targetPos.X, targetPos.Y].Notify();
                TryNotifyNeighboringChunks(pos, targetPos);

                return true;
            }

            return false;
        }
        private void TryNotifyNeighboringChunks(Vector2Int ePos, Vector2Int cPos)
        {
            if (ePos.X % DefaultChunkSize == 0 && IsWithinChunkBoundaries(new(cPos.X - 1, cPos.Y)))
                _chunks[cPos.X - 1, cPos.Y].Notify();

            if (ePos.X % DefaultChunkSize == DefaultChunkSize - 1 && IsWithinChunkBoundaries(new(cPos.X + 1, cPos.Y)))
                _chunks[cPos.X + 1, cPos.Y].Notify();

            if (ePos.Y % DefaultChunkSize == 0 && IsWithinChunkBoundaries(new(cPos.X, cPos.Y - 1)))
                _chunks[cPos.X, cPos.Y - 1].Notify();

            if (ePos.Y % DefaultChunkSize == DefaultChunkSize - 1 && IsWithinChunkBoundaries(new(cPos.X, cPos.Y + 1)))
                _chunks[cPos.X, cPos.Y + 1].Notify();
        }

        private bool IsWithinChunkBoundaries(Vector2Int pos)
        {
            return pos.X >= 0 && pos.X < worldChunkWidth &&
                   pos.Y >= 0 && pos.Y < worldChunkHeight;
        }

        internal static Vector2Int ToChunkCoordinateSystem(Vector2Int pos)
        {
            return new(pos.X / DefaultChunkSize, pos.Y / DefaultChunkSize);
        }

#if DEBUG
        private void DEBUG_DrawActiveChunks()
        {
            for (int x = 0; x < worldChunkWidth; x++)
            {
                for (int y = 0; y < worldChunkHeight; y++)
                {
                    if (_chunks[x, y].ShouldUpdate)
                    {
                        PGraphics.SpriteBatch.Draw(PTextures.Pixel, new Vector2(_chunks[x, y].Position.X, _chunks[x, y].Position.Y), null, new Color(255, 0, 0, 35), 0f, Vector2.Zero, DefaultChunkSize * PWorld.Scale, SpriteEffects.None, 0f);
                    }
                }
            }
        }
#endif
    }
}
#pragma warning restore IDE0051