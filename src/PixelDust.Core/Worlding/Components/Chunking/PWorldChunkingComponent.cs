using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Engine;

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
            _chunks = new PWorldChunk[(PWorld.Infos.Width / DefaultChunkSize) + 1,
                                      (PWorld.Infos.Height / DefaultChunkSize) + 1];

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
            // DEBUG_DrawActiveChunks();
#endif
        }

        internal bool TryGetChunkUpdateState(Vector2 pos, out bool result)
        {
            result = false;
            Vector2 targetPos = ToChunkCoordinateSystem(pos);

            if (!IsWithinChunkBoundaries(targetPos))
                return false;

            result = _chunks[(int)targetPos.X, (int)targetPos.Y].ShouldUpdate;
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
        
        internal bool TryNotifyChunk(Vector2 pos)
        {
            Vector2 targetPos = ToChunkCoordinateSystem(pos);

            if (IsWithinChunkBoundaries(targetPos))
            {
                _chunks[(int)targetPos.X, (int)targetPos.Y].Notify();
                TryNotifyNeighboringChunks(pos, targetPos);

                return true;
            }

            return false;
        }
        private void TryNotifyNeighboringChunks(Vector2 ePos, Vector2 cPos)
        {
            int cPosX = (int)cPos.X;
            int cPosY = (int)cPos.Y;

            if (ePos.X % DefaultChunkSize == 0 && IsWithinChunkBoundaries(new(cPosX - 1, cPosY)))
                _chunks[cPosX - 1, cPosY].Notify();

            if (ePos.X % DefaultChunkSize == DefaultChunkSize - 1 && IsWithinChunkBoundaries(new(cPosX + 1, cPosY)))
                _chunks[cPosX + 1, cPosY].Notify();

            if (ePos.Y % DefaultChunkSize == 0 && IsWithinChunkBoundaries(new(cPosX, cPosY - 1)))
                _chunks[cPosX, cPosY - 1].Notify();

            if (ePos.Y % DefaultChunkSize == DefaultChunkSize - 1 && IsWithinChunkBoundaries(new(cPosX, cPosY + 1)))
                _chunks[cPosX, cPosY + 1].Notify();
        }

        private bool IsWithinChunkBoundaries(Vector2 pos)
        {
            return (int)pos.X >= 0 && (int)pos.X < worldChunkWidth &&
                   (int)pos.Y >= 0 && (int)pos.Y < worldChunkHeight;
        }

        internal static Vector2 ToChunkCoordinateSystem(Vector2 pos)
        {
            int x = (int)(pos.X / DefaultChunkSize);
            int y = (int)(pos.Y / DefaultChunkSize);

            return new(x, y);
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
