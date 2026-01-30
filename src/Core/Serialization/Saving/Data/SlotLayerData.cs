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
        [IgnoreMember]
        public Color ColorModifier
        {
            get => new(ColorModifierR, ColorModifierG, ColorModifierB, ColorModifierA);

            set
            {
                ColorModifierR = value.R;
                ColorModifierG = value.G;
                ColorModifierB = value.B;
                ColorModifierA = value.A;
            }
        }

        [Key("ColorModifierA")]
        public byte ColorModifierA { get; set; }

        [Key("ColorModifierB")]
        public byte ColorModifierB { get; set; }

        [Key("ColorModifierG")]
        public byte ColorModifierG { get; set; }

        [Key("ColorModifierR")]
        public byte ColorModifierR { get; set; }

        [Key("ElementIndex")]
        public ElementIndex ElementIndex { get; set; }

        [Key("States")]
        public ElementStates States { get; set; }

        [Key("StepCycleFlag")]
        public UpdateCycleFlag StepCycleFlag { get; set; }

        [Key("StoredElementIndex")]
        public ElementIndex StoredElementIndex { get; set; }

        [Key("Temperature")]
        public float Temperature { get; set; }

        [Key("TicksRemaining")]
        public int TicksRemaining { get; set; }

        public SlotLayerData()
        {

        }

        public SlotLayerData(SlotLayer slotLayer)
        {
            ColorModifier = slotLayer.ColorModifier;
            ElementIndex = slotLayer.ElementIndex;
            States = slotLayer.States;
            StepCycleFlag = slotLayer.StepCycleFlag;
            StoredElementIndex = slotLayer.StoredElementIndex;
            Temperature = slotLayer.Temperature;
            TicksRemaining = slotLayer.TicksRemaining;
        }
    }
}

