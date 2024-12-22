using Microsoft.Xna.Framework;

using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Elements.Contexts;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Helpers;
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
        private readonly List<ISWorldSlot> capturedSlots = [];

        public override void Update(GameTime gameTime)
        {
            this.capturedSlots.Clear();

            GetAllSlotsForUpdating(gameTime);
            UpdateAllCapturedSlots(gameTime);

            this.updateCycleFlag = this.updateCycleFlag.GetNextCycle();
            this.stepCycleFlag = this.stepCycleFlag.GetNextCycle();
        }

        private void GetAllSlotsForUpdating(GameTime gameTime)
        {
            SWorldChunk[] worldChunks = this.SWorldInstance.GetActiveChunks();

            for (int i = 0; i < worldChunks.Length; i++)
            {
                SWorldChunk worldChunk = worldChunks[i];

                for (int y = 0; y < SWorldConstants.CHUNK_SCALE; y++)
                {
                    for (int x = 0; x < SWorldConstants.CHUNK_SCALE; x++)
                    {
                        Point position = new((worldChunk.Position.X / SWorldConstants.GRID_SCALE) + x, (worldChunk.Position.Y / SWorldConstants.GRID_SCALE) + y);

                        if (!this.SWorldInstance.TryGetWorldSlot(position, out ISWorldSlot worldSlot))
                        {
                            continue;
                        }

                        UpdateWorldSlotLayerTarget(gameTime, worldSlot.Position, SWorldLayer.Foreground, worldSlot, SWorldThreadUpdateType.Update);
                        UpdateWorldSlotLayerTarget(gameTime, worldSlot.Position, SWorldLayer.Background, worldSlot, SWorldThreadUpdateType.Update);

                        this.capturedSlots.Add(worldSlot);
                    }
                }
            }
        }

        private void UpdateAllCapturedSlots(GameTime gameTime)
        {
            this.capturedSlots.ForEach((worldSlot) =>
            {
                UpdateWorldSlotLayerTarget(gameTime, worldSlot.Position, SWorldLayer.Foreground, worldSlot, SWorldThreadUpdateType.Step);
                UpdateWorldSlotLayerTarget(gameTime, worldSlot.Position, SWorldLayer.Background, worldSlot, SWorldThreadUpdateType.Step);
            });
        }

        private void UpdateWorldSlotLayerTarget(GameTime gameTime, Point position, SWorldLayer worldLayer, ISWorldSlot worldSlot, SWorldThreadUpdateType updateType)
        {
            SWorldSlotLayer worldSlotLayer = (SWorldSlotLayer)worldSlot.GetLayer(worldLayer);
            SElement element = (SElement)worldSlotLayer.Element;

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
