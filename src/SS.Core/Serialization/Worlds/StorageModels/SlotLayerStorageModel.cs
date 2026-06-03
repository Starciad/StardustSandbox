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
using StardustSandbox.Core.Enums.Indexers;

namespace StardustSandbox.Core.Serialization.Worlds.StorageModels
{
    public sealed class SlotLayerStorageModel
    {
        public byte ColorModifierA { get; set; }
        public byte ColorModifierB { get; set; }
        public byte ColorModifierG { get; set; }
        public byte ColorModifierR { get; set; }
        public ElementIndex ElementIndex { get; set; }
        public ElementStates States { get; set; }
        public UpdateCycleFlag StepCycleFlag { get; set; }
        public ElementIndex StoredElementIndex { get; set; }
        public float Temperature { get; set; }

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
    }
}

