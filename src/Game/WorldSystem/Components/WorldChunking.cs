/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
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
                    this.chunks[x, y] = new Chunk(new(x * WorldConstants.CHUNK_SCALE * WorldConstants.GRID_SIZE, y * WorldConstants.CHUNK_SCALE * WorldConstants.GRID_SIZE));
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

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    Chunk chunk = this.chunks[x, y];

                    Vector2 position = new(chunk.Position.X, chunk.Position.Y);
                    Vector2 scale = new(WorldConstants.CHUNK_SCALE * WorldConstants.GRID_SIZE);
                    Color color = chunk.ShouldUpdate ? AAP64ColorPalette.Crimson : AAP64ColorPalette.White;

                    spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Pixel), position, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
            }
        }

        internal bool TryGetChunkUpdateState(in Point position, out bool result)
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

        internal bool TryNotifyChunk(in Point position)
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
        private void TryNotifyNeighboringChunks(in Point ePos, in Point cPos)
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

        private bool IsWithinChunkBoundaries(in Point position)
        {
            return position.X >= 0 && position.X < this.worldChunkWidth &&
                   position.Y >= 0 && position.Y < this.worldChunkHeight;
        }

        internal static Point ToChunkCoordinateSystem(in Point position)
        {
            return new(position.X / WorldConstants.CHUNK_SCALE, position.Y / WorldConstants.CHUNK_SCALE);
        }
    }
}

