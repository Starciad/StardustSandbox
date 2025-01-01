using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.IO.Files.World.Data;
using StardustSandbox.Core.IO.Files.World;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.World.Slots;
using StardustSandbox.Core.Interfaces.Collections;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld
    {
        #region Start New
        public void StartNew()
        {
            StartNew(this.Infos.Size);
        }

        public void StartNew(SSize2 size)
        {
            this.IsActive = true;
            this.IsVisible = true;

            if (this.Infos.Size != size)
            {
                Resize(size);
            }

            Reset();
        }
        #endregion

        #region Load From World File
        public void LoadFromWorldSaveFile(SWorldSaveFile worldSaveFile)
        {
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = true;

            // World
            StartNew(worldSaveFile.World.Size);

            // Cache
            this.currentlySelectedWorldSaveFile = worldSaveFile;

            // Metadata
            this.Infos.Identifier = worldSaveFile.Metadata.Identifier;
            this.Infos.Name = worldSaveFile.Metadata.Name;
            this.Infos.Description = worldSaveFile.Metadata.Description;

            // Allocate Elements
            foreach (SWorldSlotData worldSlotData in worldSaveFile.World.Slots)
            {
                if (worldSlotData.ForegroundLayer != null)
                {
                    LoadWorldSlotLayerData(SWorldLayer.Foreground, worldSlotData.Position, worldSlotData.ForegroundLayer);
                }

                if (worldSlotData.BackgroundLayer != null)
                {
                    LoadWorldSlotLayerData(SWorldLayer.Background, worldSlotData.Position, worldSlotData.BackgroundLayer);
                }
            }
        }
        private void LoadWorldSlotLayerData(SWorldLayer worldLayer, Point position, SWorldSlotLayerData worldSlotLayerData)
        {
            InstantiateElement(position, worldLayer, worldSlotLayerData.ElementIdentifier);

            SWorldSlot worldSlot = GetWorldSlot(position);

            worldSlot.SetTemperatureValue(worldLayer, worldSlotLayerData.Temperature);
            worldSlot.SetFreeFalling(worldLayer, worldSlotLayerData.FreeFalling);
            worldSlot.SetColorModifier(worldLayer, worldSlotLayerData.ColorModifier);
        }
        #endregion

        public void Resize(SSize2 size)
        {
            DestroyWorldSlots();

            this.Infos.Size = size;
            this.slots = new SWorldSlot[size.Width, size.Height];

            InstantiateWorldSlots();
        }

        public void Reload()
        {
            if (this.currentlySelectedWorldSaveFile != null)
            {
                LoadFromWorldSaveFile(this.currentlySelectedWorldSaveFile);
            }
            else
            {
                Clear();
            }
        }
        
        public bool InsideTheWorldDimensions(Point position)
        {
            return position.X >= 0 && position.X < this.Infos.Size.Width &&
                   position.Y >= 0 && position.Y < this.Infos.Size.Height;
        }

        #region Clear
        public void Clear()
        {
            ClearSlots();
            ClearEntities();
        }

        private void ClearSlots()
        {
            if (this.slots == null)
            {
                return;
            }

            for (int x = 0; x < this.Infos.Size.Width; x++)
            {
                for (int y = 0; y < this.Infos.Size.Height; y++)
                {
                    if (IsEmptyWorldSlot(new(x, y)))
                    {
                        continue;
                    }

                    DestroyElement(new(x, y), SWorldLayer.Foreground);
                    DestroyElement(new(x, y), SWorldLayer.Background);
                }
            }
        }

        private void ClearEntities()
        {
            RemoveAllEntity();
        }
        #endregion

        #region Build and Destroy World
        private void InstantiateWorldSlots()
        {
            if (this.slots == null || this.slots.Length == 0)
            {
                return;
            }

            for (int y = 0; y < this.Infos.Size.Height; y++)
            {
                for (int x = 0; x < this.Infos.Size.Width; x++)
                {
                    this.slots[x, y] = this.worldSlotsPool.TryGet(out ISPoolableObject value) ? (SWorldSlot)value : new();
                }
            }
        }

        private void DestroyWorldSlots()
        {
            if (this.slots == null || this.slots.Length == 0)
            {
                return;
            }

            for (int y = 0; y < this.Infos.Size.Height; y++)
            {
                for (int x = 0; x < this.Infos.Size.Width; x++)
                {
                    if (this.slots[x, y] == null)
                    {
                        continue;
                    }

                    this.worldSlotsPool.Add(this.slots[x, y]);
                    this.slots[x, y] = null;
                }
            }
        }
        #endregion
    }
}
