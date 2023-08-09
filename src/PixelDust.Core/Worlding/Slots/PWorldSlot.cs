using System.Runtime.InteropServices;

using PixelDust.Core.Elements;
using PixelDust.Core.Extensions;

using Microsoft.Xna.Framework;

namespace PixelDust.Core.Worlding
{
    [StructLayout(LayoutKind.Sequential)]
    public class PWorldSlot
    {
        public Color Color => new(_colorR, _colorG, _colorB);
        public PElement Element
        {
            get
            {
                return PElementManager.GetElementById<PElement>(_elementId);
            }
        }
        public byte ElementId => _elementId;

        // Header
        private byte _elementId;
        private byte _colorR, _colorG, _colorB;

        public PWorldSlot(uint id)
        {
            Instantiate(id);
        }

        public PWorldSlot(PElement value)
        {
            Instantiate(value);
        }

        #region Elements (Tools)
        private void Instantiate(uint id)
        {
            Instantiate(PElementManager.GetElementById<PElement>(id));
        }
        private void Instantiate(PElement value)
        {
            // Id
            _elementId = value.Id;

            // Color
            Color colorResult;
            if (value.HasColorVariation) colorResult = value.Color.Vary();
            else colorResult = value.Color;

            _colorR = colorResult.R;
            _colorG = colorResult.G;
            _colorB = colorResult.B;
        }
        #endregion
    }
}