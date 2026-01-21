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

using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class SlotLayerData
    {
        [Key("ElementIndex")]
        public ElementIndex ElementIndex { get; init; }

        [Key("StoredElementIndex")]
        public ElementIndex StoredElementIndex { get; init; }

        [Key("Temperature")]
        public float Temperature { get; init; }

        [Key("States")]
        public ElementStates States { get; init; }

        [Key("ColorModifierR")]
        public byte ColorModifierR { get; init; }

        [Key("ColorModifierG")]
        public byte ColorModifierG { get; init; }

        [Key("ColorModifierB")]
        public byte ColorModifierB { get; init; }

        [Key("ColorModifierA")]
        public byte ColorModifierA { get; init; }

        [Key("StepCycleFlag")]
        public UpdateCycleFlag StepCycleFlag { get; init; }

        [IgnoreMember]
        public Color ColorModifier
        {
            get => new(this.ColorModifierR, this.ColorModifierG, this.ColorModifierB, this.ColorModifierA);

            init
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

        public SlotLayerData(SlotLayer slotLayer)
        {
            this.ElementIndex = slotLayer.ElementIndex;
            this.Temperature = slotLayer.Temperature;
            this.States = slotLayer.States;
            this.ColorModifier = slotLayer.ColorModifier;
            this.StepCycleFlag = slotLayer.StepCycleFlag;
            this.StoredElementIndex = slotLayer.StoredElementIndex;
        }
    }
}

