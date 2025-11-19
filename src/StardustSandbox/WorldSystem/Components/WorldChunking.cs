using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Interfaces;
using StardustSandbox.WorldSystem.Chunking;

using System.Collections.Generic;

namespace StardustSandbox.WorldSystem.Components
{
    internal sealed class WorldChunking : IResettable
    {
        private int worldChunkWidth;
        private int worldChunkHeight;

        private Chunk[,] chunks;

        private readonly World world;

        public void Reset()
        {
            this.chunks = new Chunk[
                (this.world.Information.Size.X / WorldConstants.CHUNK_SCALE) + 1,
                (this.world.Information.Size.Y / WorldConstants.CHUNK_SCALE) + 1
            ];

            this.worldChunkWidth = this.chunks.GetLength(0);
            this.worldChunkHeight = this.chunks.GetLength(1);

            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    this.chunks[x, y] = new Chunk(new Point(x * WorldConstants.CHUNK_SCALE * WorldConstants.GRID_SIZE, y * WorldConstants.CHUNK_SCALE * WorldConstants.GRID_SIZE));
                }
            }
        }

        internal WorldChunking(World world)
        {
            this.world = world;

            Reset();
        }

        internal void Update()
        {
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    this.chunks[x, y].Update();
                }
            }
        }

        internal bool TryGetChunkUpdateState(Point position, out bool result)
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

        internal int GetActiveChunksCount()
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

        internal IEnumerable<Chunk> GetActiveChunks()
        {
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    Chunk worldChunk = this.chunks[x, y];

                    if (worldChunk.ShouldUpdate)
                    {
                        yield return worldChunk;
                    }
                }
            }
        }

        internal bool TryNotifyChunk(Point position)
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
            if (ePos.X % WorldConstants.CHUNK_SCALE == 0 && IsWithinChunkBoundaries(new(cPos.X - 1, cPos.Y)))
            {
                this.chunks[cPos.X - 1, cPos.Y].Notify();
            }

            if (ePos.X % WorldConstants.CHUNK_SCALE == WorldConstants.CHUNK_SCALE - 1 && IsWithinChunkBoundaries(new(cPos.X + 1, cPos.Y)))
            {
                this.chunks[cPos.X + 1, cPos.Y].Notify();
            }

            if (ePos.Y % WorldConstants.CHUNK_SCALE == 0 && IsWithinChunkBoundaries(new(cPos.X, cPos.Y - 1)))
            {
                this.chunks[cPos.X, cPos.Y - 1].Notify();
            }

            if (ePos.Y % WorldConstants.CHUNK_SCALE == WorldConstants.CHUNK_SCALE - 1 && IsWithinChunkBoundaries(new(cPos.X, cPos.Y + 1)))
            {
                this.chunks[cPos.X, cPos.Y + 1].Notify();
            }
        }

        private bool IsWithinChunkBoundaries(Point position)
        {
            return position.X >= 0 && position.X < this.worldChunkWidth &&
                   position.Y >= 0 && position.Y < this.worldChunkHeight;
        }

        internal static Point ToChunkCoordinateSystem(Point position)
        {
            return new(position.X / WorldConstants.CHUNK_SCALE, position.Y / WorldConstants.CHUNK_SCALE);
        }
    }
}
