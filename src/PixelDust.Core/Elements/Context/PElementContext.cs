using Microsoft.Xna.Framework;

using System;

using PixelDust.Core.Worlding;

namespace PixelDust.Core.Elements
{
    public sealed partial class PElementContext
    {
        internal PWorld World => _world;

        public PWorldSlot Slot => _slot;
        public PElement Element => _slot.Element;
        public Vector2 Position => _position;

        private PWorldSlot _slot;
        private Vector2 _position;

        private readonly PWorld _world;

        public PElementContext(PWorld world)
        {
            _world = world;
        }

        internal void Update(PWorldSlot slot, Vector2 position)
        {
            _slot = slot;
            _position = position;
        }
    }
}