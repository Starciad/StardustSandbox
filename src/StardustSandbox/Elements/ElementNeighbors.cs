using StardustSandbox.Enums.World;
using StardustSandbox.Interfaces;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Elements
{
    internal sealed class ElementNeighbors : IResettable
    {
        internal int Length => this.slots.Length;

        private readonly Slot[] slots;

        internal ElementNeighbors()
        {
            this.slots = new Slot[8];
        }

        internal void SetNeighbor(int index, Slot slot)
        {
            this.slots[index] = slot;
        }

        internal Slot GetSlot(int index)
        {
            return this.slots[index];
        }

        internal SlotLayer GetSlotLayer(int index, Layer layer)
        {
            Slot slot = GetSlot(index);

            return slot?.GetLayer(layer);
        }

        internal bool HasNeighbor(int index)
        {
            return this.slots[index] != null;
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
