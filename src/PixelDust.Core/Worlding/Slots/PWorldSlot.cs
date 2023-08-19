using Microsoft.Xna.Framework;

using PixelDust.Core.Elements;
using PixelDust.Core.Extensions;

using System.Runtime.InteropServices;

namespace PixelDust.Core.Worlding
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PWorldSlot
    {
        public readonly PElement Element => PElementManager.GetElementById<PElement>(id);
        public readonly Color Color => new(cR, cG, cB);

        // Header
        private byte id;
        private byte cR, cG, cB;
        private short temperature;
        private short density;

        internal PWorldSlot(byte id)
        {
            Instantiate(PElementManager.GetElementById<PElement>(id));
        }
        internal PWorldSlot(PElement value)
        {
            Instantiate(value);
        }

        internal void Instantiate(PElement value)
        {
            // id
            this.id = value.Id;

            // colors
            Color rColor = value.Color;
            if (value.HasColorVariation)
                rColor = rColor.Vary(8);

            this.cR = rColor.R;
            this.cG = rColor.G;
            this.cB = rColor.B;

            // temperature
            this.temperature = value.DefaultTemperature;
        }

        public void Copy(PWorldSlot value)
        {
            this = value;
        }
        public void Destroy()
        {
            id = 0;
            cR = 0; cG = 0; cB = 0;
        }

        public readonly bool IsEmpty()
        {
            return id == 0;
        }
    }
}