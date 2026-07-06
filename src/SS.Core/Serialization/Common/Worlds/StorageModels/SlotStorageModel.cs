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
using StardustSandbox.Core.Interfaces.Serialization.Morph;
using StardustSandbox.Core.WorldSystem.Slots;

namespace StardustSandbox.Core.Serialization.Common.Worlds.StorageModels
{
    internal sealed class SlotStorageModel : IStorageModel
    {
        internal SlotLayerStorageModel BackgroundLayer { get; set; }
        internal SlotLayerStorageModel ForegroundLayer { get; set; }
        internal int PositionX { get; set; }
        internal int PositionY { get; set; }

        internal Point Position
        {
            get => new(this.PositionX, this.PositionY);

            set
            {
                this.PositionX = value.X;
                this.PositionY = value.Y;
            }
        }

        internal SlotStorageModel()
        {
            this.BackgroundLayer = new();
            this.ForegroundLayer = new();
            this.PositionX = 0;
            this.PositionY = 0;
        }

        internal SlotStorageModel(Slot slot)
        {
            this.PositionX = slot.Position.X;
            this.PositionY = slot.Position.Y;

            SlotLayerStorageModel CreateLayerModel(Layer layer)
            {
                return slot.HasElement(layer) ? new(slot, layer) : new();
            }

            this.ForegroundLayer = CreateLayerModel(Layer.Foreground);
            this.BackgroundLayer = CreateLayerModel(Layer.Background);
        }
    }
}

