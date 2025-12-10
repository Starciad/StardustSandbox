using Microsoft.Xna.Framework;

using StardustSandbox.Constants;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.Interfaces;
using StardustSandbox.WorldSystem.Chunking;

namespace StardustSandbox.WorldSystem.Components
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

        internal void Update(in GameTime gameTime)
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

                        if (!slot.Foreground.HasState(ElementStates.IsEmpty))
                        {
                            UpdateSlotLayerTarget(gameTime, slot.Position, Layer.Foreground, slot);
                        }

                        if (!slot.Background.HasState(ElementStates.IsEmpty))
                        {
                            UpdateSlotLayerTarget(gameTime, slot.Position, Layer.Background, slot);
                        }
                    }
                }
            }

            this.stepCycleFlag = this.stepCycleFlag.GetNextCycle();
        }

        private void UpdateSlotLayerTarget(in GameTime gameTime, Point position, Layer layer, Slot slot)
        {
            SlotLayer slotLayer = slot.GetLayer(layer);
            Element element = slotLayer.Element;

            if (slotLayer == null || element == null)
            {
                return;
            }

            this.elementUpdateContext.UpdateInformation(position, layer, slot);
            element.SetContext(this.elementUpdateContext);

            if (slotLayer.StepCycleFlag == this.stepCycleFlag)
            {
                return;
            }

            slotLayer.NextStepCycle();
            element.Steps(gameTime);
        }
    }
}
