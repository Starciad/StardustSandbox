using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Engine;

using System;

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
            _chunks = new PWorldChunk[(World.Infos.Width / DefaultChunkSize) + 1,
                                      (World.Infos.Height / DefaultChunkSize) + 1];

            worldChunkWidth = _chunks.GetLength(0);
            worldChunkHeight = _chunks.GetLength(1);

            for (int x = 0; x < worldChunkWidth; x++)
            {
                for (int y = 0; y < worldChunkHeight; y++)
                {
                    _chunks[x, y] = new(new(x * DefaultChunkSize * PWorld.GridScale, y * DefaultChunkSize * PWorld.GridScale), DefaultChunkSize);
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
#endif
        }

        internal bool TryGetChunkUpdateState(Vector2 pos, out bool result)
        {
            result = false;

            if (!InsideTheChunksDimensions(pos))
                return false;

            Vector2 cPos = ToChunkCoordinateSystem(pos);
            result = _chunks[(int)cPos.X, (int)cPos.Y].ShouldUpdate;
            return true;
        }

        internal bool TryNotifyChunk(Vector2 pos)
        {
            if (!InsideTheChunksDimensions(pos))
                return false;

            Vector2 cPos = ToChunkCoordinateSystem(pos);
            NotifyNeighbors(pos, cPos);

            _chunks[(int)cPos.X, (int)cPos.Y].Notify();
            return true;
        }
        private void NotifyNeighbors(Vector2 globalPos, Vector2 localPos)
        {
            // Left
            if (globalPos.X % DefaultChunkSize == 0)
            {
                if (InsideTheChunksLocalDimensions(new(localPos.X - 1, localPos.Y)))
                    _chunks[(int)localPos.X - 1, (int)localPos.Y].Notify();
            }

            // Right
            if (globalPos.X % DefaultChunkSize == DefaultChunkSize - 1)
            {
                if (InsideTheChunksLocalDimensions(new(localPos.X + 1, localPos.Y)))
                    _chunks[(int)localPos.X + 1, (int)localPos.Y].Notify();
            }

            // Up
            if (globalPos.Y % DefaultChunkSize == 0)
            {
                if (InsideTheChunksLocalDimensions(new(localPos.X, localPos.Y - 1)))
                    _chunks[(int)localPos.X, (int)localPos.Y - 1].Notify();
            }

            // Down
            if (globalPos.Y % DefaultChunkSize == DefaultChunkSize - 1)
            {
                if (InsideTheChunksLocalDimensions(new(localPos.X, localPos.Y + 1)))
                    _chunks[(int)localPos.X, (int)localPos.Y + 1].Notify();
            }
        }

        internal bool InsideTheChunksDimensions(Vector2 pos)
        {
            return InsideTheChunksLocalDimensions(ToChunkCoordinateSystem(pos));
        }
        private bool InsideTheChunksLocalDimensions(Vector2 pos)
        {
            return (int)pos.X >= 0 && (int)pos.X < worldChunkWidth &&
                   (int)pos.Y >= 0 && (int)pos.Y < worldChunkHeight;
        }

        internal static Vector2 ToChunkCoordinateSystem(Vector2 pos)
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
                        PGraphics.SpriteBatch.Draw(PTextures.Pixel, new(_chunks[x, y].Position.X, _chunks[x, y].Position.Y), null, Color.Red, 0f, Vector2.Zero, DefaultChunkSize * PWorld.GridScale, SpriteEffects.None, 0f);
                    }
                }
            }
        }
#endif
    }
}
