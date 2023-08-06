using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;

using PixelDust.Core.Elements;
using PixelDust.Core.Extensions;

namespace PixelDust.Core.World
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PWorldSlot
    {
        public readonly Color TargetColor => _color;

        public Color _color;
        private double _temperature;
        private uint _elementId;

        public PWorldSlot()
        {
            _temperature = 0;
            _elementId = 0;

            _color = Color.Black;
        }

        #region Slot (General)
        internal void Instantiate<T>() where T : PElement
        {
            Instantiate(PElementManager.GetIdOfElement<T>());
        }

        internal void Instantiate(PElement element)
        {
            Instantiate(element.Id);
        }

        internal void Instantiate(uint id)
        {
            _elementId = id;
            PElement target = Get();

            // Temperature
            _temperature = target.DefaultTemperature;

            // Color
            if (target.HasColorVariation) _color = target.Color.Vary();
            else _color = target.Color;
        }

        internal void Destroy()
        {
            _elementId = 0;
        }

        internal readonly PElement Get()
        {
            if (IsEmpty())
                return null;

            return PElementManager.GetElementById<PElement>(_elementId);
        }

        internal readonly bool IsEmpty()
        {
            return _elementId == 0;
        }
        #endregion
    }
}