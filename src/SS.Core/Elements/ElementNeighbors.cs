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

using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.WorldSystem.Slots;

namespace StardustSandbox.Core.Elements
{
    internal sealed class ElementNeighbors : IResettable
    {
        internal int Length => this.slots.Length;
        internal int CountOccupied { get; private set; }

        // [0] Northwest
        // [1] North
        // [2] Northeast
        // [3] West
        // [4] East
        // [5] Southwest
        // [6] South
        // [7] Southeast

        private readonly Slot[] slots = new Slot[8];

        internal void SetNeighbor(int index, Slot slot)
        {
            this.slots[index] = slot;
        }

        internal void SetNeighbor(in ElementNeighborDirection direction, Slot slot)
        {
            SetNeighbor((int)direction, slot);
        }

        internal void SetNeighborCountOccupied(int countOccupied)
        {
            this.CountOccupied = countOccupied;
        }

        internal Slot GetSlot(int index)
        {
            return this.slots[index];
        }

        internal Slot GetSlot(in ElementNeighborDirection direction)
        {
            return GetSlot((int)direction);
        }

        internal bool HasNeighbor(int index)
        {
            return this.slots[index] != null;
        }

        internal bool HasNeighbor(in ElementNeighborDirection direction)
        {
            return HasNeighbor((int)direction);
        }

        internal int CountNeighborsByElementIndex(ElementIndex elementIndex, Layer layer)
        {
            int count = 0;

            for (int i = 0; i < this.slots.Length; i++)
            {
                if (IsNeighborLayerOccupied(i, layer) && GetSlot(i).GetElementIndex(layer) == elementIndex)
                {
                    count++;
                }
            }

            return count;
        }

        internal bool IsNeighborLayerOccupied(int index, Layer layer)
        {
            return HasNeighbor(index) && GetSlot(index).HasElement(layer);
        }

        internal bool IsNeighborLayerOccupied(in ElementNeighborDirection direction, Layer layer)
        {
            return IsNeighborLayerOccupied((int)direction, layer);
        }

        internal Point GetNeighborPosition(int index)
        {
            return this.slots[index]?.Position ?? new(-1);
        }

        internal Point GetNeighborPosition(in ElementNeighborDirection direction)
        {
            return GetNeighborPosition((int)direction);
        }

        public void Reset()
        {
            for (int i = 0; i < this.slots.Length; i++)
            {
                this.slots[i] = null;
            }

            this.CountOccupied = 0;
        }

        internal static bool IsDiagonalNeighbor(int index)
        {
            return index is (int)ElementNeighborDirection.Northwest or (int)ElementNeighborDirection.Northeast or (int)ElementNeighborDirection.Southwest or (int)ElementNeighborDirection.Southeast;
        }

        internal static bool IsDiagonalNeighbor(in ElementNeighborDirection direction)
        {
            return IsDiagonalNeighbor((int)direction);
        }
    }
}
