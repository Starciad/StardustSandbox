using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;

namespace PixelDust.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WorldSlot
    {
        public readonly Color TargetColor => _color;

        public Color _color;
        private double _temperature;
        private uint _elementId;

        public WorldSlot()
        {
            _temperature = 0;
            _elementId = 0;

            _color = Color.Black;
        }

        #region Slot (General)
        internal void Instantiate<T>() where T : PElement
        {
            Instantiate(PElements.GetIdOfElement<T>());
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

            return PElements.GetElementById<PElement>(_elementId);
        }
        internal readonly bool IsEmpty()
        {
            return _elementId == 0;
        }
        #endregion
    }
}