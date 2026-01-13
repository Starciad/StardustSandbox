/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Interfaces;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Elements
{
    internal sealed class ElementNeighbors : IResettable
    {
        internal int Length => this.slots.Length;

        /*
         * [0] Northwest
         * [1] North
         * [2] Northeast
         * [3] West
         * [4] East
         * [5] Southwest
         * [6] South
         * [7] Southeast
        */
        private readonly Slot[] slots;

        internal ElementNeighbors()
        {
            this.slots = new Slot[8];
        }

        internal void SetNeighbor(in int index, Slot slot)
        {
            this.slots[index] = slot;
        }

        internal void SetNeighbor(in ElementNeighborDirection direction, Slot slot)
        {
            SetNeighbor((int)direction, slot);
        }

        internal Slot GetSlot(in int index)
        {
            return this.slots[index];
        }

        internal Slot GetSlot(in ElementNeighborDirection direction)
        {
            return GetSlot((int)direction);
        }

        internal SlotLayer GetSlotLayer(in int index, in Layer layer)
        {
            return GetSlot(index)?.GetLayer(layer);
        }

        internal SlotLayer GetSlotLayer(in ElementNeighborDirection direction, in Layer layer)
        {
            return GetSlotLayer((int)direction, layer);
        }

        internal bool HasNeighbor(in int index)
        {
            return this.slots[index] != null;
        }

        internal bool HasNeighbor(in ElementNeighborDirection direction)
        {
            return HasNeighbor((int)direction);
        }

        internal int CountNeighborsByElementIndex(in ElementIndex elementIndex, in Layer layer)
        {
            int count = 0;

            for (int i = 0; i < this.Length; i++)
            {
                if (IsNeighborLayerOccupied(i, layer) && GetSlotLayer(i, layer).ElementIndex == elementIndex)
                {
                    count++;
                }
            }

            return count;
        }

        internal bool IsNeighborLayerOccupied(in int index, in Layer layer)
        {
            return HasNeighbor(index) && !GetSlotLayer(index, layer).IsEmpty;
        }

        internal bool IsNeighborLayerOccupied(in ElementNeighborDirection direction, in Layer layer)
        {
            return IsNeighborLayerOccupied((int)direction, layer);
        }

        public void Reset()
        {
            for (int i = 0; i < this.Length; i++)
            {
                this.slots[i] = null;
            }
        }
    }
}

