using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Elements.Contexts;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.World;

using System.Collections.Generic;

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
