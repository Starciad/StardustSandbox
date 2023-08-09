using Microsoft.Xna.Framework;

using PixelDust.Core.Elements;

namespace PixelDust.Core.Worlding
{
    public static partial class PWorld
    {
        public static bool TryInstantiate<T>(Vector2 pos) where T : PElement
        {
            return TryInstantiate(pos, PElementManager.GetIdOfElement<T>());
        }
        public static bool TryInstantiate(Vector2 pos, uint id)
        {
            return TryInstantiate(pos, PElementManager.GetElementById<PElement>(id));
        }
        public static bool TryInstantiate(Vector2 pos, PElement value)
        {
            if (!InsideTheWorldDimensions(pos) || !IsEmpty(pos))
                return false;

            TryNotifyChunk(pos);

            Slots[(int)pos.X, (int)pos.Y] = new(value);
            return true;
        }

        public static bool TryUpdatePosition(Vector2 oldPos, Vector2 newPos)
        {
            if (!InsideTheWorldDimensions(oldPos) ||
                !InsideTheWorldDimensions(newPos) ||
                IsEmpty(oldPos) ||
                !IsEmpty(newPos))
                return false;

            TryNotifyChunk(oldPos);
            TryNotifyChunk(newPos);

            Slots[(int)newPos.X, (int)newPos.Y] = Slots[(int)oldPos.X, (int)oldPos.Y];
            Slots[(int)oldPos.X, (int)oldPos.Y] = null;
            return true;
        }

        public static bool TrySwitchPosition(Vector2 oldPos, Vector2 newPos)
        {
            if (!InsideTheWorldDimensions(oldPos) ||
                !InsideTheWorldDimensions(newPos) ||
                IsEmpty(oldPos) ||
                IsEmpty(newPos))
                return false;

            TryNotifyChunk(oldPos);
            TryNotifyChunk(newPos);

            (Slots[(int)newPos.X, (int)newPos.Y], Slots[(int)oldPos.X, (int)oldPos.Y]) =
            (Slots[(int)oldPos.X, (int)oldPos.Y], Slots[(int)newPos.X, (int)newPos.Y]);

            return true;
        }

        public static bool TryDestroy(Vector2 pos)
        {
            if (!InsideTheWorldDimensions(pos) ||
                IsEmpty(pos))
                return false;

            TryNotifyChunk(pos);

            Slots[(int)pos.X, (int)pos.Y] = null;
            return true;
        }

        public static bool TryGetElement(Vector2 pos, out PElement value)
        {
            if (!InsideTheWorldDimensions(pos) ||
                IsEmpty(pos))
            {
                value = null;
                return false;
            }

            value = Slots[(int)pos.X, (int)pos.Y].Element;
            return true;
        }

        public static bool TryGetSlot(Vector2 pos, out PWorldSlot slot)
        {
            slot = null;
            if (!InsideTheWorldDimensions(pos))
                return false;

            slot = Slots[(int)pos.X, (int)pos.Y];
            return slot != null;
        }

        public static bool IsEmpty(Vector2 pos)
        {
            if (!InsideTheWorldDimensions(pos) || Slots[(int)pos.X, (int)pos.Y] == null)
                return true;
            
            return false;
        }
    }
}