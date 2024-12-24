using Microsoft.Xna.Framework;

using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements.Contexts;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Helpers;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.World.Data;

using System.Collections.Generic;

namespace StardustSandbox.Core.Components.Common.World
{
    public sealed class SWorldUpdatingComponent(ISGame gameInstance, ISWorld worldInstance) : SWorldComponent(gameInstance, worldInstance)
    {
        private SUpdateCycleFlag updateCycleFlag;
        private SUpdateCycleFlag stepCycleFlag;

        private readonly SElementContext elementUpdateContext = new(worldInstance);

        public override void Update(GameTime gameTime)
        {
            foreach (SWorldSlot worldSlot in GetAllSlotsForUpdating())
            {
                if (!worldSlot.ForegroundLayer.IsEmpty)
                {
                    UpdateWorldSlotLayerTarget(gameTime, worldSlot.Position, SWorldLayer.Foreground, worldSlot, SWorldThreadUpdateType.Update);
                    UpdateWorldSlotLayerTarget(gameTime, worldSlot.Position, SWorldLayer.Foreground, worldSlot, SWorldThreadUpdateType.Step);
                }

                if (!worldSlot.BackgroundLayer.IsEmpty)
                {
                    UpdateWorldSlotLayerTarget(gameTime, worldSlot.Position, SWorldLayer.Background, worldSlot, SWorldThreadUpdateType.Update);
                    UpdateWorldSlotLayerTarget(gameTime, worldSlot.Position, SWorldLayer.Background, worldSlot, SWorldThreadUpdateType.Step);
                }
            }

            this.updateCycleFlag = this.updateCycleFlag.GetNextCycle();
            this.stepCycleFlag = this.stepCycleFlag.GetNextCycle();
        }

        private IEnumerable<SWorldSlot> GetAllSlotsForUpdating()
        {
            foreach (SWorldChunk worldChunk in this.SWorldInstance.GetActiveChunks())
            {
                for (int y = 0; y < SWorldConstants.CHUNK_SCALE; y++)
                {
                    for (int x = 0; x < SWorldConstants.CHUNK_SCALE; x++)
                    {
                        Point position = new((worldChunk.Position.X / SWorldConstants.GRID_SCALE) + x, (worldChunk.Position.Y / SWorldConstants.GRID_SCALE) + y);

                        if (!this.SWorldInstance.TryGetWorldSlot(position, out SWorldSlot worldSlot))
                        {
                            continue;
                        }

                        yield return worldSlot;
                    }
                }
            }
        }

        private void UpdateWorldSlotLayerTarget(GameTime gameTime, Point position, SWorldLayer worldLayer, SWorldSlot worldSlot, SWorldThreadUpdateType updateType)
        {
            SWorldSlotLayer worldSlotLayer = worldSlot.GetLayer(worldLayer);
            ISElement element = worldSlotLayer.Element;

            if (worldSlotLayer == null || element == null)
            {
                return;
            }

            this.elementUpdateContext.UpdateInformation(position, worldLayer, worldSlot);
            element.Context = this.elementUpdateContext;

            switch (updateType)
            {
                case SWorldThreadUpdateType.Update:
                    if (worldSlotLayer.UpdateCycleFlag == this.updateCycleFlag)
                    {
                        break;
                    }

                    worldSlotLayer.NextUpdateCycle();
                    element.Update(gameTime);
                    break;

                case SWorldThreadUpdateType.Step:
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
