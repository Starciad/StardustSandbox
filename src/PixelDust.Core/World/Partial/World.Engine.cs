using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace PixelDust.Core
{
    public sealed partial class World
    {
        private static int TotalWorldThreads => 6;
        private static int WorldThreadSize { get;set ; }

        private static readonly WorldThreadInfo[] _worldThreadsInfos = new WorldThreadInfo[TotalWorldThreads];

        // Chunks
        #region Initialization
        public void Initialize()
        {
            GenerateUpdatesMultithreaded();
        }
        private void GenerateUpdatesMultithreaded()
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
        #endregion

        #region Update
        public void Update()
        {
            if (IsPaused || isUnloaded) return;
            UpdateWorldThreads();
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
            List<Vector2> _capturedSlots = new();
            uint totalCapturedElements = 0;

            // Find slots
            for (int x = 0; x < threadInfo.Range + 1; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Vector2 pos = new(x + threadInfo.StartPosition, y);
                    if (_slots[(int)pos.X, (int)pos.Y].IsEmpty()) continue;

                    _capturedSlots.Add(pos);
                    totalCapturedElements++;
                }
            }

            // Update slots
            for (int i = 0; i < totalCapturedElements; i++)
            {
                Vector2 pos = _capturedSlots[i];
                WorldSlot slot = new();
                TryGetSlot(pos, ref slot);

                _EContext.Update(slot, pos);
                if (TryGetElement(pos, out PElement value))
                {
                    value?.Update(_EContext);
                }
            }

            _capturedSlots.Clear();
        }
        #endregion

        #region Draw
        public void Draw()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (IsEmpty(new(x, y))) continue;

                    PGraphics.SpriteBatch.Draw(
                        PTextures.Pixel,
                        new Vector2(x * GridScale, y * GridScale),
                        null,
                        _slots[x, y].TargetColor,
                        0f,
                        Vector2.Zero,
                        GridScale,
                        SpriteEffects.None,
                        0f
                    );
                }
            }
        }
        #endregion
    }
}
