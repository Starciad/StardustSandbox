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

        internal void Update(GameTime gameTime)
        {
            foreach (Chunk worldChunk in this.world.GetActiveChunks())
            {
                for (int y = 0; y < WorldConstants.CHUNK_SCALE; y++)
                {
                    for (int x = 0; x < WorldConstants.CHUNK_SCALE; x++)
                    {
                        Point position = new((worldChunk.Position.X / WorldConstants.GRID_SIZE) + x, (worldChunk.Position.Y / WorldConstants.GRID_SIZE) + y);

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

            this.stepCycleFlag = this.stepCycleFlag.GetNextCycle();
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
    }
}

