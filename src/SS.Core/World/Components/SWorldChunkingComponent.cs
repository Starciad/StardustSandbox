using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.World.Data;
using StardustSandbox.Game.World;

namespace StardustSandbox.Core.World.Components
{
    public sealed class SWorldChunkingComponent(ISGame gameInstance, SWorld worldInstance) : SWorldComponent(gameInstance, worldInstance)
    {
        private int worldChunkWidth;
        private int worldChunkHeight;

        private SWorldChunk[,] _chunks;

        private Texture2D pixelTexture;

        public override void Initialize()
        {
            this.pixelTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");

            this._chunks = new SWorldChunk[(int)(this.SWorldInstance.Infos.Size.Width / SWorldConstants.CHUNK_SCALE) + 1, (int)(this.SWorldInstance.Infos.Size.Height / SWorldConstants.CHUNK_SCALE) + 1];

            this.worldChunkWidth = this._chunks.GetLength(0);
            this.worldChunkHeight = this._chunks.GetLength(1);

            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    this._chunks[x, y] = new SWorldChunk(new Point(x * SWorldConstants.CHUNK_SCALE * SWorldConstants.GRID_SCALE, y * SWorldConstants.CHUNK_SCALE * SWorldConstants.GRID_SCALE));
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    this._chunks[x, y].Update();
                }
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
#if DEBUG
            // Debug methods
            // DEBUG_DrawActiveChunks(spriteBatch);
#endif
        }

        public bool TryGetChunkUpdateState(Point pos, out bool result)
        {
            result = false;
            Point targetPos = ToChunkCoordinateSystem(pos);

            if (!IsWithinChunkBoundaries(targetPos))
            {
                return false;
            }

            result = this._chunks[targetPos.X, targetPos.Y].ShouldUpdate;
            return true;
        }
        public int GetActiveChunksCount()
        {
            int result = 0;
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    if (this._chunks[x, y].ShouldUpdate)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public bool TryNotifyChunk(Point pos)
        {
            Point targetPos = ToChunkCoordinateSystem(pos);

            if (IsWithinChunkBoundaries(targetPos))
            {
                this._chunks[targetPos.X, targetPos.Y].Notify();
                TryNotifyNeighboringChunks(pos, targetPos);

                return true;
            }

            return false;
        }
        private void TryNotifyNeighboringChunks(Point ePos, Point cPos)
        {
            if (ePos.X % SWorldConstants.CHUNK_SCALE == 0 && IsWithinChunkBoundaries(new(cPos.X - 1, cPos.Y)))
            {
                this._chunks[cPos.X - 1, cPos.Y].Notify();
            }

            if (ePos.X % SWorldConstants.CHUNK_SCALE == SWorldConstants.CHUNK_SCALE - 1 && IsWithinChunkBoundaries(new(cPos.X + 1, cPos.Y)))
            {
                this._chunks[cPos.X + 1, cPos.Y].Notify();
            }

            if (ePos.Y % SWorldConstants.CHUNK_SCALE == 0 && IsWithinChunkBoundaries(new(cPos.X, cPos.Y - 1)))
            {
                this._chunks[cPos.X, cPos.Y - 1].Notify();
            }

            if (ePos.Y % SWorldConstants.CHUNK_SCALE == SWorldConstants.CHUNK_SCALE - 1 && IsWithinChunkBoundaries(new(cPos.X, cPos.Y + 1)))
            {
                this._chunks[cPos.X, cPos.Y + 1].Notify();
            }
        }

        private bool IsWithinChunkBoundaries(Point pos)
        {
            return pos.X >= 0 && pos.X < this.worldChunkWidth &&
                   pos.Y >= 0 && pos.Y < this.worldChunkHeight;
        }

        public static Point ToChunkCoordinateSystem(Point pos)
        {
            return new(pos.X / SWorldConstants.CHUNK_SCALE, pos.Y / SWorldConstants.CHUNK_SCALE);
        }

#if DEBUG
        private void DEBUG_DrawActiveChunks(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    if (this._chunks[x, y].ShouldUpdate)
                    {
                        spriteBatch.Draw(this.pixelTexture, new Vector2(this._chunks[x, y].Position.X, this._chunks[x, y].Position.Y), null, new Color(255, 0, 0, 35), 0f, Vector2.Zero, SWorldConstants.CHUNK_SCALE * SWorldConstants.GRID_SCALE, SpriteEffects.None, 0f);
                    }
                }
            }
        }
#endif
    }
}