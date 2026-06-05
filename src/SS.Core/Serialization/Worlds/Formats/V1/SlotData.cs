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

using MessagePack;

using StardustSandbox.Core.Interfaces.Serialization.Worlds;
using StardustSandbox.Core.WorldSystem.Slots;

using System;

namespace StardustSandbox.Core.Serialization.Worlds.Formats.V1
{
    [Serializable]
    [MessagePackObject]
    public sealed class SlotData : IData
    {
        [Key(0)]
        public SlotLayerData BackgroundLayer { get; set; }

        [Key(1)]
        public SlotLayerData ForegroundLayer { get; set; }

        [Key(2)]
        public int PositionX { get; set; }

        [Key(3)]
        public int PositionY { get; set; }

        public SlotData()
        {

        }

        internal SlotData(Slot slot)
        {
            this.PositionX = slot.Position.X;
            this.PositionY = slot.Position.Y;

            if (!slot.Foreground.IsEmpty)
            {
                this.ForegroundLayer = new(slot.Foreground);
            }

            if (!slot.Background.IsEmpty)
            {
                this.BackgroundLayer = new(slot.Background);
            }
        }
    }
}

