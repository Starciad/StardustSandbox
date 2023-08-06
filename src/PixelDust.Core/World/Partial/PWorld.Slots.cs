using Microsoft.Xna.Framework;

using System;

using PixelDust.Core.Elements;

namespace PixelDust.Core.World
{
    public sealed partial class PWorld
    {
        public bool TryInstantiate<T>(Vector2 pos) where T : PElement
        {
            if (!InsideTheWorldDimensions(pos) ||
                !IsEmpty(pos))
                return false;

            _slots[(int)pos.X, (int)pos.Y].Instantiate<T>();
            return true;
        }

        public bool TryInstantiate(uint id, Vector2 pos)
        {
            if (!InsideTheWorldDimensions(pos) ||
                !IsEmpty(pos))
                return false;

            _slots[(int)pos.X, (int)pos.Y].Instantiate(id);
            return true;
        }

        public bool TryUpdatePosition(Vector2 oldPos, Vector2 newPos)
        {
            if (!InsideTheWorldDimensions(oldPos) ||
                !InsideTheWorldDimensions(newPos) ||
                IsEmpty(oldPos) ||
                !IsEmpty(newPos))
                return false;

            PElement targetElement = _slots[(int)oldPos.X, (int)oldPos.Y].Get();

            _slots[(int)newPos.X, (int)newPos.Y].Instantiate(targetElement);
            _slots[(int)oldPos.X, (int)oldPos.Y].Destroy();
            return true;
        }

        public bool TrySwitchPosition(Vector2 oldPos, Vector2 newPos)
        {
            if (!InsideTheWorldDimensions(oldPos) ||
                !InsideTheWorldDimensions(newPos) ||
                IsEmpty(oldPos) ||
                IsEmpty(newPos))
                return false;

            PElement e1 = _slots[(int)oldPos.X, (int)oldPos.Y].Get();
            PElement e2 = _slots[(int)newPos.X, (int)newPos.Y].Get();

            _slots[(int)newPos.X, (int)newPos.Y].Instantiate(e1);
            _slots[(int)oldPos.X, (int)oldPos.Y].Instantiate(e2);
            return true;
        }

        public bool TryDestroy(Vector2 pos)
        {
            if (!InsideTheWorldDimensions(pos) ||
                IsEmpty(pos))
                return false;

            _slots[(int)pos.X, (int)pos.Y].Destroy();
            return true;
        }

        public bool TryGetElement(Vector2 pos, out PElement value)
        {
            if (!InsideTheWorldDimensions(pos) ||
                IsEmpty(pos))
            {
                value = null;
                return false;
            }

            value = _slots[(int)pos.X, (int)pos.Y].Get();
            return true;
        }

        public bool TryGetSlot(Vector2 pos, ref PWorldSlot slot)
        {
            if (!InsideTheWorldDimensions(pos))
                return false;

            slot = _slots[(int)pos.X, (int)pos.Y];
            return true;
        }

        public bool TryModifySlot(Vector2 pos, Func<PWorldSlot, PWorldSlot> function)
        {
            if (!InsideTheWorldDimensions(pos))
                return false;

            _slots[(int)pos.X, (int)pos.Y] = function.Invoke(_slots[(int)pos.X, (int)pos.Y]);
            return true;
        }

        public bool IsEmpty(Vector2 pos)
        {
            if (!InsideTheWorldDimensions(pos))
                return false;

            return _slots[(int)pos.X, (int)pos.Y].IsEmpty();
        }
    }
}
