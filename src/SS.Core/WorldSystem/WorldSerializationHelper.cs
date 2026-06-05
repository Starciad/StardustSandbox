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
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Worlds.Formats.V1;
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

        internal SlotData[] Serialize()
        {
            List<SlotData> slots = [];

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
            WorldSaveFile saveFile = this.worldSerializer.Load(saveFileName, LoadFlags.Metadata | LoadFlags.Properties | LoadFlags.Environment | LoadFlags.Content);

            // World
            this.world.StartNew(saveFile.Properties.Size);

            // Metadata
            this.world.Name = saveFile.Metadata.Name;
            this.world.Description = saveFile.Metadata.Description;

            // Time
            this.world.Time.SetTime(saveFile.Environment.CurrentTime);
            this.world.Time.IsFrozen = saveFile.Environment.IsFrozen;

            // Temperature
            this.world.Temperature.Deserialize(saveFile.Environment.Temperatures);

            // Allocate Slots
            foreach (SlotData slotData in saveFile.Content.Slots)
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

        private void LoadSlotLayerData(Layer layer, Point position, SlotLayerData slotLayerData)
        {
            this.tileMap.InstantiateElementIndex(position, layer, slotLayerData.ElementIndex);

            Slot slot = this.tileMap.GetSlot(position);

            slot.SetTemperatureValue(layer, slotLayerData.Temperature);
            slot.SetState(layer, slotLayerData.States);
            slot.SetColorModifier(layer, slotLayerData.ColorModifier);
            slot.SetStoredElement(layer, slotLayerData.StoredElementIndex);
        }
    }
}
