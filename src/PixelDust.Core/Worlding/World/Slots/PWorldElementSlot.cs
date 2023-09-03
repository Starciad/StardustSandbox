using PixelDust.Core.Elements;

namespace PixelDust.Core.Worlding
{
    public struct PWorldElementSlot
    {
        public readonly PElement Instance => PElementsHandler.GetElementById<PElement>(id);
        public readonly bool IsEmpty => id == 0;

        private byte id;

        internal void Instantiate(uint id)
        {
            Instantiate(PElementsHandler.GetElementById(id));
        }
        internal void Instantiate(PElement value)
        {
            Reset();
            id = value.Id;
        }
        internal void Destroy()
        {
            Reset();
            id = 0;
        }
        internal void Copy(PWorldElementSlot value)
        {
            this = value;
        }

        private void Reset()
        {

        }
    }
}