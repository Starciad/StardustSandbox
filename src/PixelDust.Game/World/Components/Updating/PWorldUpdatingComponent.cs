using Microsoft.Xna.Framework;

using PixelDust.Game.Elements;
using PixelDust.Game.Elements.Contexts;
using PixelDust.Game.Enums.World;
using PixelDust.Game.Mathematics;
using PixelDust.Game.World.Slots;

using System;
using System.Collections.Generic;

namespace PixelDust.Game.World.Components.Threading
{
    public sealed class PWorldUpdatingComponent : PWorldComponent
    {
        private readonly List<Vector2Int> capturedSlots = [];

        protected override void OnUpdate(GameTime gameTime)
        {
            capturedSlots.Clear();

            GetAllElementsForUpdating(gameTime);
            UpdateAllCapturedElements(gameTime);
        }

        private void GetAllElementsForUpdating(GameTime gameTime)
        {
            for (int y = 0; y < this.World.Infos.Size.Height; y++)
            {
                for (int x = 0; x < this.World.Infos.Size.Width; x++)
                {
                    Vector2Int pos = new(x, y);
                    bool chunkState = this.World.GetChunkUpdateState(pos);

                    if (this.World.IsEmptyElementSlot(pos) || !chunkState)
                    {
                        continue;
                    }
                    else
                    {
                        UpdateElementTarget(gameTime, pos, PWorldThreadUpdateType.Update);
                        capturedSlots.Add(pos);
                    }
                }
            }
        }

        private void UpdateAllCapturedElements(GameTime gameTime)
        {
            int totalCapturedSlots = capturedSlots.Count;
            for (int i = 0; i < totalCapturedSlots; i++)
            {
                UpdateElementTarget(gameTime, capturedSlots[i], PWorldThreadUpdateType.Step);
            }
        }

        private void UpdateElementTarget(GameTime gameTime, Vector2Int position, PWorldThreadUpdateType updateType)
        {
            PWorldElementSlot slot = this.World.GetElementSlot(position);

            if (this.World.TryGetElement(position, out PElement value))
            {
                value.Context = new PElementContext(this.World, this.World.ElementDatabase, slot, position);

                switch (updateType)
                {
                    case PWorldThreadUpdateType.Update:
                        value.Update(gameTime);
                        break;

                    case PWorldThreadUpdateType.Step:
                        value.Steps();
                        break;

                    default:
                        return;
                }
            }
        }
    }
}
