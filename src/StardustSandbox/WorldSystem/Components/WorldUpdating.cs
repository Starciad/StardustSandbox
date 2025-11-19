using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.Interfaces;
using StardustSandbox.WorldSystem.Chunking;

using System.Collections.Generic;

namespace StardustSandbox.WorldSystem.Components
{
    internal sealed class WorldUpdating(World world) : IResettable
    {
        private UpdateCycleFlag updateCycleFlag;
        private UpdateCycleFlag stepCycleFlag;

        private readonly ElementContext elementUpdateContext = new(world);
        private readonly World world = world;

        public void Reset()
        {
            this.updateCycleFlag = UpdateCycleFlag.None;
            this.stepCycleFlag = UpdateCycleFlag.None;
        }

        internal void Update(GameTime gameTime)
        {
            foreach (Slot worldSlot in GetAllSlotsForUpdating())
            {
                if (!worldSlot.ForegroundLayer.IsEmpty)
                {
                    UpdateSlotLayerTarget(gameTime, worldSlot.Position, LayerType.Foreground, worldSlot, UpdateType.Update);
                    UpdateSlotLayerTarget(gameTime, worldSlot.Position, LayerType.Foreground, worldSlot, UpdateType.Step);
                }

                if (!worldSlot.BackgroundLayer.IsEmpty)
                {
                    UpdateSlotLayerTarget(gameTime, worldSlot.Position, LayerType.Background, worldSlot, UpdateType.Update);
                    UpdateSlotLayerTarget(gameTime, worldSlot.Position, LayerType.Background, worldSlot, UpdateType.Step);
                }
            }

            this.updateCycleFlag = this.updateCycleFlag.GetNextCycle();
            this.stepCycleFlag = this.stepCycleFlag.GetNextCycle();
        }

        private IEnumerable<Slot> GetAllSlotsForUpdating()
        {
            foreach (Chunk worldChunk in this.world.GetActiveChunks())
            {
                for (int y = 0; y < WorldConstants.CHUNK_SCALE; y++)
                {
                    for (int x = 0; x < WorldConstants.CHUNK_SCALE; x++)
                    {
                        Point position = new((worldChunk.Position.X / WorldConstants.GRID_SIZE) + x, (worldChunk.Position.Y / WorldConstants.GRID_SIZE) + y);

                        if (!this.world.TryGetSlot(position, out Slot worldSlot))
                        {
                            continue;
                        }

                        yield return worldSlot;
                    }
                }
            }
        }

        private void UpdateSlotLayerTarget(GameTime gameTime, Point position, LayerType worldLayer, Slot worldSlot, UpdateType updateType)
        {
            SlotLayer worldSlotLayer = worldSlot.GetLayer(worldLayer);
            Element element = worldSlotLayer.Element;

            if (worldSlotLayer == null || element == null)
            {
                return;
            }

            this.elementUpdateContext.UpdateInformation(position, worldLayer, worldSlot);
            element.Context = this.elementUpdateContext;

            switch (updateType)
            {
                case UpdateType.Update:
                    if (worldSlotLayer.UpdateCycleFlag == this.updateCycleFlag)
                    {
                        break;
                    }

                    worldSlotLayer.NextUpdateCycle();
                    element.Update(gameTime);
                    break;

                case UpdateType.Step:
                    if (worldSlotLayer.StepCycleFlag == this.stepCycleFlag)
                    {
                        break;
                    }

                    worldSlotLayer.NextStepCycle();
                    element.Steps();
                    break;

                default:
                    return;
            }
        }
    }
}
