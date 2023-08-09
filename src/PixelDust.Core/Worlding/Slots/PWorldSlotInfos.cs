using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;

using PixelDust.Core.Elements;
using PixelDust.Core.Extensions;

namespace PixelDust.Core.Worlding
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class PWorldSlotInfos
    {
        private PElement EInstance { get; set; }
        public Color Color => new(_colorR, _colorG, _colorB);
        public short Temperature => _temperature;
        public short Density => _density;
        public bool IsFreeFalling => _isFreeFalling;

        private byte _colorR, _colorG, _colorB;
        private short _temperature;
        private short _density;
        private bool _isFreeFalling;

        internal void Instantiate(PElement value)
        {
            EInstance = value;

            BuildColors();
            BuildTemperature();
            BuildDensity();
        }
        internal void Destroy()
        {
            _colorR = 0;
            _colorG = 0;
            _colorB = 0;
            _temperature = 0;
            _density = 0;
        }

        // Builders
        private void BuildColors()
        {
            Color colorResult;
            if (EInstance.HasColorVariation) colorResult = EInstance.Color.Vary(8);
            else colorResult = EInstance.Color;

            _colorR = colorResult.R;
            _colorG = colorResult.G;
            _colorB = colorResult.B;
        }
        private void BuildTemperature()
        {
            _temperature = EInstance.DefaultTemperature;
        }
        private void BuildDensity()
        {
            _density = EInstance.DefaultDensity;
        }

        // Tools
        public void SetFreeFalling(bool value)
        {
            _isFreeFalling = value;
        }
    }
}
