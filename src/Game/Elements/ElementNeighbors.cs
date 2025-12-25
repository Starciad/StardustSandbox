using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Interfaces;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Elements
{
    internal sealed class ElementNeighbors : IResettable
    {
        internal int Length => this.slots.Length;

        // [0] Northwest
        // [1] North
        // [2] Northeast
        // [3] East
        // [4] Southeast
        // [5] South
        // [6] Southwest
        // [7] West
        private readonly Slot[] slots;

        internal ElementNeighbors()
        {
            this.slots = new Slot[8];
        }

        internal void SetNeighbor(in int index, Slot slot)
        {
            this.slots[index] = slot;
        }

        internal Slot GetSlot(in int index)
        {
            return this.slots[index];
        }

        internal SlotLayer GetSlotLayer(in int index, in Layer layer)
        {
            Slot slot = GetSlot(index);

            return slot?.GetLayer(layer);
        }

        internal bool HasNeighbor(in int index)
        {
            return this.slots[index] != null;
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

        internal bool IsNeighborLayerOccupied(int index, Layer layer)
        {
            return HasNeighbor(index) && !GetSlotLayer(index, layer).IsEmpty;
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
