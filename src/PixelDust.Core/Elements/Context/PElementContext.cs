using Microsoft.Xna.Framework;

using System;

using PixelDust.Core.Worlding;

namespace PixelDust.Core.Elements
{
    public sealed partial class PElementContext
    {
        public PWorldSlot Slot => _slot;
        public PElement Element => _slot.Element;
        public Vector2 Position => _position;

        private PWorldSlot _slot;
        private Vector2 _position;

        internal void Update(PWorldSlot slot, Vector2 position)
        {
            _slot = slot;
            _position = position;
        }
    }
}