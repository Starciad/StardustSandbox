using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Elements.Contexts;
using StardustSandbox.Game.Enums.World;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.World.Data;

using System.Collections.Generic;

namespace StardustSandbox.Game.World.Components
{
    public sealed class SWorldUpdatingComponent(ISGame gameInstance, SWorld worldInstance) : SWorldComponent(gameInstance, worldInstance)
    {
        private readonly SElementContext elementUpdateContext = new(worldInstance, gameInstance.ElementDatabase);
        private readonly List<Point> capturedSlots = [];

        public override void Update(GameTime gameTime)
        {
            this.capturedSlots.Clear();

            GetAllElementsForUpdating(gameTime);
            UpdateAllCapturedElements(gameTime);
        }

        private void GetAllElementsForUpdating(GameTime gameTime)
        {
            for (int y = 0; y < this.SWorldInstance.Infos.Size.Height; y++)
            {
                for (int x = 0; x < this.SWorldInstance.Infos.Size.Width; x++)
                {
                    Point pos = new(x, y);
                    bool chunkState = this.SWorldInstance.GetChunkUpdateState(pos);

                    if (this.SWorldInstance.IsEmptyElementSlot(pos) || !chunkState)
                    {
                        continue;
                    }
                    else
                    {
                        UpdateElementTarget(gameTime, pos, SWorldThreadUpdateType.Update);
                        this.capturedSlots.Add(pos);
                    }
                }
            }
        }

        private void UpdateAllCapturedElements(GameTime gameTime)
        {
            int totalCapturedSlots = this.capturedSlots.Count;
            for (int i = 0; i < totalCapturedSlots; i++)
            {
                UpdateElementTarget(gameTime, this.capturedSlots[i], SWorldThreadUpdateType.Step);
            }
        }

        private void UpdateElementTarget(GameTime gameTime, Point position, SWorldThreadUpdateType updateType)
        {
            SWorldSlot slot = this.SWorldInstance.GetElementSlot(position);

            if (this.SWorldInstance.TryGetElement(position, out SElement value))
            {
                this.elementUpdateContext.UpdateInformation(slot, position);
                value.Context = this.elementUpdateContext;

                switch (updateType)
                {
                    case SWorldThreadUpdateType.Update:
                        value.Update(gameTime);
                        break;

                    case SWorldThreadUpdateType.Step:
                        value.Steps();
                        break;

                    default:
                        return;
                }
            }
        }
    }
}
