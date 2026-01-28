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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces;

using System;

namespace StardustSandbox.Core.WorldSystem
{
    internal sealed class WorldUpdating(World world) : IResettable
    {
        private UpdateCycleFlag stepCycleFlag;

        private readonly ElementContext elementUpdateContext = new(world);
        private readonly World world = world;

        public void Reset()
        {
            this.stepCycleFlag = UpdateCycleFlag.None;
        }

        private void UpdateSlotLayerTarget(GameTime gameTime, in Point position, in Layer layer, Slot slot)
        {
            SlotLayer slotLayer = slot.GetLayer(layer);

            this.elementUpdateContext.UpdateInformation(position, layer, slot);
            slotLayer.Element.SetContext(this.elementUpdateContext);

            if (slotLayer.StepCycleFlag == this.stepCycleFlag)
            {
                return;
            }

            slotLayer.NextStepCycle();
            slotLayer.Element.Steps(gameTime);
        }

        private void UpdateChunk(GameTime gameTime, Chunk chunk)
        {
            for (int y = 0; y < WorldConstants.CHUNK_SCALE; y++)
            {
                for (int x = 0; x < WorldConstants.CHUNK_SCALE; x++)
                {
                    Point position = new((chunk.Position.X / WorldConstants.GRID_SIZE) + x, (chunk.Position.Y / WorldConstants.GRID_SIZE) + y);

                    if (!this.world.TryGetSlot(position, out Slot slot))
                    {
                        continue;
                    }

                    if (!slot.Foreground.IsEmpty)
                    {
                        UpdateSlotLayerTarget(gameTime, slot.Position, Layer.Foreground, slot);
                    }

                    if (!slot.Background.IsEmpty)
                    {
                        UpdateSlotLayerTarget(gameTime, slot.Position, Layer.Background, slot);
                    }
                }
            }
        }

        internal void Update(GameTime gameTime)
        {
            foreach (Chunk chunk in this.world.GetActiveChunks())
            {
                UpdateChunk(gameTime, chunk);
            }

            this.stepCycleFlag = this.stepCycleFlag.GetNextCycle();
        }
    }
}

