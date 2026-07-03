/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Serialization.Common.Worlds;
using StardustSandbox.Core.Serialization.Common.Worlds.StorageModels;
using StardustSandbox.Core.WorldSystem.Components;
using StardustSandbox.Core.WorldSystem.Slots;

using System.Collections.Generic;

namespace StardustSandbox.Core.WorldSystem
{
    internal sealed class WorldSerializationHelper
    {
        private readonly TileMap tileMap;
        private readonly World world;
        private readonly WorldSerializer worldSerializer;

        internal WorldSerializationHelper(World world, WorldSerializer worldSerializer)
        {
            this.tileMap = world.TileMap;
            this.world = world;
            this.worldSerializer = worldSerializer;
        }

        internal SlotStorageModel[] Serialize()
        {
            List<SlotStorageModel> slots = [];

            for (int y = 0; y < this.tileMap.Height; y++)
            {
                for (int x = 0; x < this.tileMap.Width; x++)
                {
                    Point point = new(x, y);

                    if (this.tileMap.IsEmptySlot(point))
                    {
                        continue;
                    }

                    slots.Add(new(this.tileMap.GetSlot(point)));
                }
            }

            return [.. slots];
        }

        internal void Deserialize(string saveFileName)
        {
            ContentStorageModel content = this.worldSerializer.Load<ContentStorageModel>(saveFileName);
            EnvironmentStorageModel environment = this.worldSerializer.Load<EnvironmentStorageModel>(saveFileName);
            ManifestStorageModel manifest = this.worldSerializer.Load<ManifestStorageModel>(saveFileName);
            PropertyStorageModel property = this.worldSerializer.Load<PropertyStorageModel>(saveFileName);

            // World
            this.world.StartNew(property.Size);

            // Metadata
            this.world.Name = manifest.Name;
            this.world.Description = manifest.Description;

            // Time
            this.world.Time.SetTime(environment.CurrentTime);
            this.world.Time.IsFrozen = environment.IsFrozen;

            // Allocate Slots
            foreach (SlotStorageModel slotData in content.Slots)
            {
                if (slotData.ForegroundLayer != null)
                {
                    LoadSlotLayerData(Layer.Foreground, slotData.Position, slotData.ForegroundLayer);
                }

                if (slotData.BackgroundLayer != null)
                {
                    LoadSlotLayerData(Layer.Background, slotData.Position, slotData.BackgroundLayer);
                }
            }
        }

        private void LoadSlotLayerData(Layer layer, Point position, SlotLayerStorageModel slotLayer)
        {
            this.tileMap.Instantiate(position, layer, slotLayer.ElementIndex);

            Slot slot = this.tileMap.GetSlot(position);

            slot.SetTemperatureValue(layer, slotLayer.Temperature);
            slot.SetColorModifier(layer, slotLayer.ColorModifier);
            slot.SetStoredElement(layer, slotLayer.StoredElementIndex);
        }
    }
}
