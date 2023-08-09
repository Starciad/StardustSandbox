using PixelDust.Core.Elements;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using System;

namespace PixelDust.Core.Worlding
{
    internal sealed class PWorldThreadingComponent : PWorldComponent
    {
        private static int TotalWorldThreads => 9;
        private static int WorldThreadSize { get; set; }

        private readonly PWorldThread[] _worldThreadsInfos = new PWorldThread[TotalWorldThreads];

        protected override void OnInitialize()
        {
            int totalValue = PWorld.Infos.Width;
            int remainingValue = totalValue;

            WorldThreadSize = (int)MathF.Ceiling(totalValue / TotalWorldThreads);

            // Setting Ranges
            int rangeStart = 0;
            for (int i = 0; i < TotalWorldThreads; i++)
            {
                _worldThreadsInfos[i] = new(i + 1, rangeStart, rangeStart + WorldThreadSize - 1);

                rangeStart = _worldThreadsInfos[i].EndPosition + 1;
                remainingValue -= WorldThreadSize;
            }

            // Distribute the remaining value to the last object
            if (remainingValue > 0)
            {
                _worldThreadsInfos[TotalWorldThreads - 1].EndPosition += remainingValue;
            }
        }

        protected override void OnUpdate()
        {
            if (PWorld.GetActiveChunksCount() == 0)
                return;

            // Odds
            Task odds = Task.Run(() =>
            {
                for (int i = 0; i < TotalWorldThreads; i++)
                {
                    if (i % 2 == 0)
                    {
                        ExecuteThreadColumn(_worldThreadsInfos[i]);
                    }
                }

                return Task.CompletedTask;
            });
            odds.Wait();

            // Even
            Task even = Task.Run(() =>
            {
                for (int i = 0; i < TotalWorldThreads; i++)
                {
                    if (i % 2 != 0)
                    {
                        ExecuteThreadColumn(_worldThreadsInfos[i]);
                    }
                }

                return Task.CompletedTask;
            });
            even.Wait();
        }

        // THREAD
        private void ExecuteThreadColumn(PWorldThread threadInfo)
        {
            List<Vector2> _capturedSlots = null;
            uint totalCapturedElements = 0;

            // Find slots
            for (int x = 0; x < threadInfo.Range + 1; x++)
            {
                for (int y = 0; y < PWorld.Infos.Height; y++)
                {
                    Vector2 pos = new(x + threadInfo.StartPosition, y);
                    PWorld.TryGetChunkUpdateState(pos, out bool chunkState);

                    if (PWorld.IsEmpty(pos) || !chunkState) 
                        continue;

                    _capturedSlots ??= new();

                    _capturedSlots.Add(pos);
                    totalCapturedElements++;
                }
            }

            // Update slots (Steps)
            for (int i = 0; i < totalCapturedElements; i++)
            {
                Vector2 pos = _capturedSlots[i];
                PWorld.TryGetSlot(pos, out PWorldSlot slot);

                PWorld.ElementContext.Update(slot, pos);
                if (PWorld.TryGetElement(pos, out PElement value))
                {
                    value?.Update(PWorld.ElementContext);
                }
            }

            _capturedSlots?.Clear();
        }
    }
}
