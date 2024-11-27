using Microsoft.Xna.Framework;

using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Elements.Contexts;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.World.Data;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StardustSandbox.Core.World.Components
{
    public sealed class SWorldUpdatingComponent(ISGame gameInstance, SWorld worldInstance) : SWorldComponent(gameInstance, worldInstance)
    {
        private readonly SElementContext elementUpdateContext = new(worldInstance);
        private readonly List<Point> capturedSlots = [];

        public override void Update(GameTime gameTime)
        {
            this.capturedSlots.Clear();
            GetAllElementsForUpdating(gameTime);
            UpdateAllCapturedElements(gameTime);
        }

        private void GetAllElementsForUpdating(GameTime gameTime)
        {
            SWorldChunk[] worldChunks = this.SWorldInstance.GetActiveChunks();

            for (int i = 0; i < worldChunks.Length; i++)
            {
                SWorldChunk worldChunk = worldChunks[i];

                for (int y = 0; y < SWorldConstants.CHUNK_SCALE; y++)
                {
                    for (int x = 0; x < SWorldConstants.CHUNK_SCALE; x++)
                    {
                        Point pos = new(worldChunk.Position.X / SWorldConstants.GRID_SCALE + x, worldChunk.Position.Y / SWorldConstants.GRID_SCALE + y);

                        if (this.SWorldInstance.IsEmptyElementSlot(pos))
                        {
                            continue;
                        }

                        UpdateElementTarget(gameTime, pos, SWorldThreadUpdateType.Update);
                        this.capturedSlots.Add(pos);
                    }
                }
            }
        }

        private void UpdateAllCapturedElements(GameTime gameTime)
        {
            this.capturedSlots.ForEach(x => UpdateElementTarget(gameTime, x, SWorldThreadUpdateType.Step));
        }

        private void UpdateElementTarget(GameTime gameTime, Point position, SWorldThreadUpdateType updateType)
        {
            ISWorldSlot slot = this.SWorldInstance.GetElementSlot(position);

            if (this.SWorldInstance.TryGetElement(position, out ISElement value))
            {
                this.elementUpdateContext.UpdateInformation(slot, position);
                value.Context = this.elementUpdateContext;

                switch (updateType)
                {
                    case SWorldThreadUpdateType.Update:
                        ((SElement)value).Update(gameTime);
                        break;

                    case SWorldThreadUpdateType.Step:
                        ((SElement)value).Steps();
                        break;

                    default:
                        return;
                }
            }
        }
    }
}
