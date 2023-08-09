using Microsoft.Xna.Framework;

using System;

using PixelDust.Core.Worlding;

namespace PixelDust.Core.Elements
{
    public sealed partial class PElementContext
    {
        public bool TryInstantiate<T>(Vector2 pos) where T : PElement
        {
            return _world.TryInstantiate<T>(pos);
        }

        public bool TrySetPosition(Vector2 pos)
        {
            if (_world.TryUpdatePosition(_position, pos))
            {
                TryGetSlot(pos, out _slot);
                Update(_slot, _position);

                return true;
            }

            return false;
        }

        public bool TrySwitchPosition(Vector2 oldPos, Vector2 newPos)
        {
            if (_world.TrySwitchPosition(oldPos, newPos))
            {
                TryGetSlot(newPos, out _slot);
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

        public bool TryGetSlot(Vector2 pos, out PWorldSlot value)
        {
            return _world.TryGetSlot(pos, out value);
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