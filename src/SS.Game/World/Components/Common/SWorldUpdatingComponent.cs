using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Elements.Contexts;
using StardustSandbox.Game.Enums.World;
using StardustSandbox.Game.World.Data;

using System.Collections.Generic;

namespace StardustSandbox.Game.World.Components.Common
{
    public sealed class SWorldUpdatingComponent : SWorldComponent
    {
        private readonly List<Point> capturedSlots = [];

        protected override void OnUpdate(GameTime gameTime)
        {
            this.capturedSlots.Clear();

            GetAllElementsForUpdating(gameTime);
            UpdateAllCapturedElements(gameTime);
        }

        private void GetAllElementsForUpdating(GameTime gameTime)
        {
            for (int y = 0; y < this.World.Infos.Size.Height; y++)
            {
                for (int x = 0; x < this.World.Infos.Size.Width; x++)
                {
                    Point pos = new(x, y);
                    bool chunkState = this.World.GetChunkUpdateState(pos);

                    if (this.World.IsEmptyElementSlot(pos) || !chunkState)
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
            SWorldSlot slot = this.World.GetElementSlot(position);

            if (this.World.TryGetElement(position, out SElement value))
            {
                value.Context = new SElementContext(this.World, this.World.ElementDatabase, slot, position);

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
