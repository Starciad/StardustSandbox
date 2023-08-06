using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace PixelDust.Core
{
    public sealed partial class World
    {
        private static int TotalWorldThreads => 12;
        private int WorldThreadSize { get; set; }

        private readonly WorldThreadInfo[] _worldThreadsInfos = new WorldThreadInfo[TotalWorldThreads];

        private void BuildWorldThreads()
        {
            int totalValue = (int)Width;
            int remainingValue = totalValue;

            WorldThreadSize = (totalValue / TotalWorldThreads);

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

        private void UpdateWorldThreads()
        {
            // Odds
            Task odds = Task.Run(() =>
            {
                for (int i = 0; i < TotalWorldThreads; i++)
                {
                    if (i % 2 == 0)
                    {
                        RunWorldThread(_worldThreadsInfos[i]);
                    }
                }
            });

            odds.Wait();

            // Even
            Task even = Task.Run(() =>
            {
                for (int i = 0; i < TotalWorldThreads; i++)
                {
                    if (i % 2 != 0)
                    {
                        RunWorldThread(_worldThreadsInfos[i]);
                    }
                }
            });

            even.Wait();
        }

        private void RunWorldThread(WorldThreadInfo threadInfo)
        {
            List<Vector2> capturedSlots = new();
            uint totalCapturedElements = 0;

            // Find slots
            for (int x = 0; x < threadInfo.Range + 1; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Vector2 pos = new(x + threadInfo.StartPosition, y);
                    if (_slots[(int)pos.X, (int)pos.Y].IsEmpty()) continue;

                    capturedSlots.Add(pos);
                    totalCapturedElements++;
                }
            }

            // Update slots
            for (int i = 0; i < totalCapturedElements; i++)
            {
                Vector2 pos = capturedSlots[i];
                WorldSlot slot = new();

                TryGetSlot(pos, ref slot);
                
                _EContext.Update(slot, pos);
                if (TryGetElement(pos, out PElement value))
                {
                    value?.Update(_EContext);
                }
            }

            capturedSlots.Clear();
        }
    }
}
