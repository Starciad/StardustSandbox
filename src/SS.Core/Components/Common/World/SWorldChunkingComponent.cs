using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.World.Chunking;

using System.Collections.Generic;

namespace StardustSandbox.Core.Components.Common.World
{
    public sealed class SWorldChunkingComponent(ISGame gameInstance, ISWorld worldInstance) : SWorldComponent(gameInstance, worldInstance)
    {
        private int worldChunkWidth;
        private int worldChunkHeight;

        private SWorldChunk[,] chunks;

        private Texture2D pixelTexture;

        public override void Initialize()
        {
            this.pixelTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
        }

        public override void Reset()
        {
            base.Reset();

            this.chunks = new SWorldChunk[(this.SWorldInstance.Infos.Size.Width / SWorldConstants.CHUNK_SCALE) + 1, (this.SWorldInstance.Infos.Size.Height / SWorldConstants.CHUNK_SCALE) + 1];

            this.worldChunkWidth = this.chunks.GetLength(0);
            this.worldChunkHeight = this.chunks.GetLength(1);

            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    this.chunks[x, y] = new SWorldChunk(new Point(x * SWorldConstants.CHUNK_SCALE * SWorldConstants.GRID_SIZE, y * SWorldConstants.CHUNK_SCALE * SWorldConstants.GRID_SIZE));
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    this.chunks[x, y].Update();
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

        public bool TryGetChunkUpdateState(Point position, out bool result)
        {
            result = false;
            Point targetPosition = ToChunkCoordinateSystem(position);

            if (!IsWithinChunkBoundaries(targetPosition))
            {
                return false;
            }

            result = this.chunks[targetPosition.X, targetPosition.Y].ShouldUpdate;
            return true;
        }

        public int GetActiveChunksCount()
        {
            int result = 0;

            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    if (this.chunks[x, y].ShouldUpdate)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public IEnumerable<SWorldChunk> GetActiveChunks()
        {
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    SWorldChunk worldChunk = this.chunks[x, y];

                    if (worldChunk.ShouldUpdate)
                    {
                        yield return worldChunk;
                    }
                }
            }
        }

        public bool TryNotifyChunk(Point position)
        {
            Point targetPosition = ToChunkCoordinateSystem(position);

            if (IsWithinChunkBoundaries(targetPosition))
            {
                this.chunks[targetPosition.X, targetPosition.Y].Notify();
                TryNotifyNeighboringChunks(position, targetPosition);

                return true;
            }

            return false;
        }
        private void TryNotifyNeighboringChunks(Point ePos, Point cPos)
        {
            if (ePos.X % SWorldConstants.CHUNK_SCALE == 0 && IsWithinChunkBoundaries(new(cPos.X - 1, cPos.Y)))
            {
                this.chunks[cPos.X - 1, cPos.Y].Notify();
            }

            if (ePos.X % SWorldConstants.CHUNK_SCALE == SWorldConstants.CHUNK_SCALE - 1 && IsWithinChunkBoundaries(new(cPos.X + 1, cPos.Y)))
            {
                this.chunks[cPos.X + 1, cPos.Y].Notify();
            }

            if (ePos.Y % SWorldConstants.CHUNK_SCALE == 0 && IsWithinChunkBoundaries(new(cPos.X, cPos.Y - 1)))
            {
                this.chunks[cPos.X, cPos.Y - 1].Notify();
            }

            if (ePos.Y % SWorldConstants.CHUNK_SCALE == SWorldConstants.CHUNK_SCALE - 1 && IsWithinChunkBoundaries(new(cPos.X, cPos.Y + 1)))
            {
                this.chunks[cPos.X, cPos.Y + 1].Notify();
            }
        }

        private bool IsWithinChunkBoundaries(Point position)
        {
            return position.X >= 0 && position.X < this.worldChunkWidth &&
                   position.Y >= 0 && position.Y < this.worldChunkHeight;
        }

        public static Point ToChunkCoordinateSystem(Point position)
        {
            return new(position.X / SWorldConstants.CHUNK_SCALE, position.Y / SWorldConstants.CHUNK_SCALE);
        }

#if DEBUG
        private void DEBUG_DrawActiveChunks(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    if (this.chunks[x, y].ShouldUpdate)
                    {
                        spriteBatch.Draw(this.pixelTexture, new Vector2(this.chunks[x, y].Position.X, this.chunks[x, y].Position.Y), null, new Color(255, 0, 0, 35), 0f, Vector2.Zero, SWorldConstants.CHUNK_SCALE * SWorldConstants.GRID_SIZE, SpriteEffects.None, 0f);
                    }
                }
            }
        }
#endif
    }
}