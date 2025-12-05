using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.Interfaces;
using StardustSandbox.World.Chunking;

namespace StardustSandbox.World.Components
{
    internal sealed class WorldUpdating(GameWorld world) : IResettable
    {
        private UpdateCycleFlag updateCycleFlag;
        private UpdateCycleFlag stepCycleFlag;

        private readonly ElementContext elementUpdateContext = new(world);
        private readonly GameWorld world = world;

        public void Reset()
        {
            this.updateCycleFlag = UpdateCycleFlag.None;
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

                        if (!this.world.TryGetSlot(position, out Slot worldSlot))
                        {
                            continue;
                        }

                        if (!worldSlot.ForegroundLayer.HasState(ElementStates.IsEmpty))
                        {
                            UpdateSlotLayerTarget(gameTime, worldSlot.Position, LayerType.Foreground, worldSlot);
                        }

                        if (!worldSlot.BackgroundLayer.HasState(ElementStates.IsEmpty))
                        {
                            UpdateSlotLayerTarget(gameTime, worldSlot.Position, LayerType.Background, worldSlot);
                        }
                    }
                }
            }

            this.updateCycleFlag = this.updateCycleFlag.GetNextCycle();
            this.stepCycleFlag = this.stepCycleFlag.GetNextCycle();
        }

        private void UpdateSlotLayerTarget(GameTime gameTime, Point position, LayerType layer, Slot worldSlot)
        {
            SlotLayer worldSlotLayer = worldSlot.GetLayer(layer);
            Element element = worldSlotLayer.Element;

            if (worldSlotLayer == null || element == null)
            {
                return;
            }

            this.elementUpdateContext.UpdateInformation(position, layer, worldSlot);
            element.SetContext(this.elementUpdateContext);

            if (worldSlotLayer.StepCycleFlag == this.stepCycleFlag)
            {
                return;
            }

            worldSlotLayer.NextStepCycle();
            element.Steps(gameTime);
        }
    }
}
