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
        private UpdateCycleFlag stepCycleFlag;

        private readonly ElementContext elementUpdateContext = new(world);
        private readonly GameWorld world = world;

        public void Reset()
        {
            this.stepCycleFlag = UpdateCycleFlag.None;
        }

        internal void Update(in GameTime gameTime)
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

                        if (!worldSlot.Foreground.HasState(ElementStates.IsEmpty))
                        {
                            UpdateSlotLayerTarget(gameTime, worldSlot.Position, Layer.Foreground, worldSlot);
                        }

                        if (!worldSlot.Background.HasState(ElementStates.IsEmpty))
                        {
                            UpdateSlotLayerTarget(gameTime, worldSlot.Position, Layer.Background, worldSlot);
                        }
                    }
                }
            }

            this.stepCycleFlag = this.stepCycleFlag.GetNextCycle();
        }

        private void UpdateSlotLayerTarget(in GameTime gameTime, Point position, Layer layer, Slot worldSlot)
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
