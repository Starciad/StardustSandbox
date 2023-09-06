using PixelDust.Core.Elements;
using PixelDust.Mathematics;

using System.Collections.Generic;
using System.Threading.Tasks;

using System;

namespace PixelDust.Core.Worlding
{
    internal sealed class PWorldThreadingComponent : PWorldComponent
    {
        private const int TOTAL_WORLD_THREADS = 12;

        private int _worldThreadSize;
        private readonly PWorldThread[] _worldThreadsInfos = new PWorldThread[TOTAL_WORLD_THREADS];

        protected override void OnInitialize()
        {
            int totalValue = WorldInstance.Infos.Size.Width;
            int remainingValue = totalValue;

            _worldThreadSize = (int)MathF.Ceiling(totalValue / TOTAL_WORLD_THREADS);

            // Setting Ranges
            int rangeStart = 0;
            for (int i = 0; i < TOTAL_WORLD_THREADS; i++)
            {
                _worldThreadsInfos[i] = new(i + 1, rangeStart, rangeStart + _worldThreadSize - 1);

                rangeStart = _worldThreadsInfos[i].EndPosition + 1;
                remainingValue -= _worldThreadSize;
            }

            // Distribute the remaining value to the last object
            if (remainingValue > 0)
            {
                _worldThreadsInfos[TOTAL_WORLD_THREADS - 1].EndPosition += remainingValue;
            }
        }
        protected override void OnUpdate()
        {
            // Odds
            Task odds = Task.Run(() =>
            {
                for (int i = 0; i < TOTAL_WORLD_THREADS; i++)
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
                for (int i = 0; i < TOTAL_WORLD_THREADS; i++)
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
            List<Vector2Int> _capturedSlots = null;
            uint totalCapturedElements = 0;

            // Find slots
            for (int x = 0; x < threadInfo.Range + 1; x++)
            {
                for (int y = 0; y < WorldInstance.Infos.Size.Height; y++)
                {
                    Vector2Int pos = new(x + threadInfo.StartPosition, y);
                    WorldInstance.TryGetChunkUpdateState(pos, out bool chunkState);

                    PUpdateElementTarget(pos, 1);

                    if (WorldInstance.IsEmptyElementSlot(pos) || !chunkState) 
                        continue;

                    _capturedSlots ??= new();

                    _capturedSlots.Add(pos);
                    totalCapturedElements++;
                }
            }

            // Update slots (Steps)
            for (int i = 0; i < totalCapturedElements; i++)
            {
                PUpdateElementTarget(_capturedSlots[i], 2);
            }

            _capturedSlots?.Clear();
        }
        private void PUpdateElementTarget(Vector2Int position, int updateType)
        {
            WorldInstance.TryGetElementSlot(position, out PWorldElementSlot slot);
            WorldInstance.elementUpdateContext.Update(slot, position);

            if (WorldInstance.TryGetElement(position, out PElement value))
            {
                switch (updateType)
                {
                    case 1:
                        value?.Update(WorldInstance.elementUpdateContext);
                        break;

                    case 2:
                        value?.Steps(WorldInstance.elementUpdateContext);
                        break;

                    default:
                        return;
                }
            }
        }
    }
}
