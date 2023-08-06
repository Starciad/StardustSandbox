using Microsoft.Xna.Framework;

using System;

using PixelDust.Core.World;

namespace PixelDust.Core.Elements
{
    public class PElementContext
    {
        public PElement Element => _element;
        public Vector2 Position => _position;

        private PElement _element;
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
            _element = slot.Get();
            _position = position;
        }

        public bool TryInstantiate<T>(Vector2 pos) where T : PElement
        {
            return _world.TryInstantiate<T>(pos);
        }

        public bool TrySetPosition(Vector2 pos)
        {
            if (_world.TryUpdatePosition(_position, pos))
            {
                TryGetSlot(pos, ref _slot);
                Update(_slot, _position);

                return true;
            }

            return false;
        }

        public bool TrySwitchPosition(Vector2 oldPos, Vector2 newPos)
        {
            if (_world.TrySwitchPosition(oldPos, newPos))
            {
                TryGetSlot(newPos, ref _slot);
                Update(_slot, newPos);

                return true;
            }

            return false;
        }

        public bool TryDestroy(Vector2 pos)
        {
            return _world.TryDestroy(pos);
        }

        public bool TryGetElement(Vector2 pos, out PElement value)
        {
            return _world.TryGetElement(pos, out value);
        }

        public bool TryGetSlot(Vector2 pos, ref PWorldSlot slot)
        {
            return _world.TryGetSlot(pos, ref slot);
        }

        public bool TryModifySlot(Vector2 pos, Func<PWorldSlot, PWorldSlot> function)
        {
            return _world.TryModifySlot(pos, function);
        }

        public bool TryReplace<T>(Vector2 pos) where T : PElement
        {
            if (!TryDestroy(pos)) return false;
            if (!TryInstantiate<T>(pos)) return false;

            return true;
        }

        public bool IsEmpty(Vector2 pos)
        {
            return _world.IsEmpty(pos);
        }
    }
}