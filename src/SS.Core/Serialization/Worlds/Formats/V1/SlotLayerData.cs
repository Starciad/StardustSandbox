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

using Microsoft.Xna.Framework;

using StardustSandbox.Core.WorldSystem.Slots;

using System;

namespace StardustSandbox.Core.Serialization.Worlds.Formats.V1
{
    [Serializable]
    [MessagePackObject]
    public sealed class SlotLayerData
    {
        [Key(0)]
        public byte ColorModifierA { get; set; }

        [Key(1)]
        public byte ColorModifierB { get; set; }

        [Key(2)]
        public byte ColorModifierG { get; set; }

        [Key(3)]
        public byte ColorModifierR { get; set; }

        [Key(4)]
        public byte ElementIndex { get; set; }

        [Key(5)]
        public byte StepCycleFlag { get; set; }

        [Key(6)]
        public byte StoredElementIndex { get; set; }

        [Key(7)]
        public float Temperature { get; set; }

        [Key(8)]
        public bool IsFalling { get; set; }

        [Key(9)]
        public bool WasPushed { get; set; }

        [Key(10)]
        public bool IsDissipating { get; set; }

        [IgnoreMember]
        public Color ColorModifier
        {
            get => new(this.ColorModifierR, this.ColorModifierG, this.ColorModifierB, this.ColorModifierA);

            set
            {
                this.ColorModifierR = value.R;
                this.ColorModifierG = value.G;
                this.ColorModifierB = value.B;
                this.ColorModifierA = value.A;
            }
        }

        public SlotLayerData()
        {

        }

        internal SlotLayerData(SlotLayer slotLayer)
        {
            this.ColorModifier = slotLayer.ColorModifier;
            this.ElementIndex = (byte)slotLayer.ElementIndex;
            this.StepCycleFlag = (byte)slotLayer.StepCycleFlag;
            this.StoredElementIndex = (byte)slotLayer.StoredElementIndex;
            this.Temperature = slotLayer.Temperature;
            this.IsFalling = slotLayer.IsFalling;
            this.WasPushed = slotLayer.WasPushed;
            this.IsDissipating = slotLayer.IsDissipating;
        }
    }
}

