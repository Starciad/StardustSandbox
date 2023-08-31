using PixelDust.Core.Elements;

using System;
using System.Runtime.InteropServices;

namespace PixelDust.Core.Worlding
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PWorldSlot : ICloneable
    {
        public readonly PElement Element => PElementsHandler.GetElementById<PElement>(id);

        // Header
        private byte id;

        internal PWorldSlot(byte id)
        {
            Instantiate(PElementsHandler.GetElementById<PElement>(id));
        }
        internal PWorldSlot(PElement value)
        {
            Instantiate(value);
        }

        internal void Instantiate(PElement value)
        {
            // id
            id = value.Id;
        }

        public void Copy(PWorldSlot value)
        {
            this = value;
        }
        public void Destroy()
        {
            id = 0;
        }

        public readonly bool IsEmpty()
        {
            return id == 0;
        }

        public readonly object Clone()
        {
            return this;
        }
    }
}