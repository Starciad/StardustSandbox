using PixelDust.Core.Elements;
using PixelDust.Mathematics;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PixelDust.Core.Worlding
{
    public sealed partial class PWorld
    {
        public bool TryInstantiateElement<T>(Vector2Int pos) where T : PElement
        {
            return TryInstantiateElement(pos, (uint)PElementsHandler.GetIdOfElementType<T>());
        }
        public bool TryInstantiateElement(Vector2Int pos, uint id)
        {
            return TryInstantiateElement(pos, PElementsHandler.GetElementById(id));
        }
        public bool TryInstantiateElement(Vector2Int pos, PElement value)
        {
            if (!InsideTheWorldDimensions(pos) || !IsEmptyElementSlot(pos))
                return false;

            TryNotifyChunk(pos);

            Elements[pos.X, pos.Y].Instantiate(value);
            return true;
        }

        public bool TryUpdateElementPosition(Vector2Int oldPos, Vector2Int newPos)
        {
            if (!InsideTheWorldDimensions(oldPos) ||
                !InsideTheWorldDimensions(newPos) ||
                IsEmptyElementSlot(oldPos) ||
                !IsEmptyElementSlot(newPos))
                return false;

            TryNotifyChunk(oldPos);
            TryNotifyChunk(newPos);

            Elements[newPos.X, newPos.Y].Copy(Elements[oldPos.X, oldPos.Y]);
            Elements[oldPos.X, oldPos.Y].Destroy();
            return true;
        }

        public bool TrySwappingElements(Vector2Int element1, Vector2Int element2)
        {
            if (!InsideTheWorldDimensions(element1) ||
                !InsideTheWorldDimensions(element2) ||
                IsEmptyElementSlot(element1) ||
                IsEmptyElementSlot(element2))
                return false;

            TryNotifyChunk(element1);
            TryNotifyChunk(element2);

            PWorldElementSlot oldValue = Elements[element1.X, element1.Y];
            PWorldElementSlot newValue = Elements[element2.X, element2.Y];

            Elements[element1.X, element1.Y].Copy(newValue);
            Elements[element2.X, element2.Y].Copy(oldValue);

            return true;
        }

        public bool TryDestroyElement(Vector2Int pos)
        {
            if (!InsideTheWorldDimensions(pos) ||
                IsEmptyElementSlot(pos))
                return false;

            TryNotifyChunk(pos);
            Elements[pos.X, pos.Y].Destroy();

            return true;
        }

        public bool TryReplaceElement<T>(Vector2Int pos) where T : PElement
        {
            if (!TryDestroyElement(pos)) return false;
            if (!TryInstantiateElement<T>(pos)) return false;

            return true;
        }

        public bool TryGetElement(Vector2Int pos, out PElement value)
        {
            if (!InsideTheWorldDimensions(pos) ||
                IsEmptyElementSlot(pos))
            {
                value = null;
                return false;
            }

            value = Elements[pos.X, pos.Y].Instance;
            return true;
        }

        public bool TryGetElementNeighbors(Vector2Int pos, out ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors)
        {
            neighbors = default;

            if (!InsideTheWorldDimensions(pos))
                return false;

            Vector2Int[] neighborsPositions = GetElementNeighborPositions(pos);

            var slotsFound = new (Vector2Int, PWorldElementSlot)[neighborsPositions.Length];
            int count = 0;

            foreach (Vector2Int position in neighborsPositions)
            {
                if (TryGetElementSlot(position, out PWorldElementSlot value))
                {
                    slotsFound[count] = (position, value);
                    count++;
                }
            }

            if (count > 0)
            {
                neighbors = new ReadOnlySpan<(Vector2Int, PWorldElementSlot)>(slotsFound, 0, count);
                return true;
            }

            return false;
        }
        private static Vector2Int[] GetElementNeighborPositions(Vector2Int pos)
        {
            return new Vector2Int[]
            {
                new(pos.X, pos.Y - 1),
                new(pos.X + 1, pos.Y - 1),
                new(pos.X - 1, pos.Y - 1),
                new(pos.X + 1, pos.Y),
                new(pos.X - 1, pos.Y),
                new(pos.X, pos.Y + 1),
                new(pos.X + 1, pos.Y + 1),
                new(pos.X - 1, pos.Y + 1),
            };
        }

        public bool TryGetElementSlot(Vector2Int pos, out PWorldElementSlot value)
        {
            value = default;
            if (!InsideTheWorldDimensions(pos))
                return false;

            value = Elements[pos.X, pos.Y];
            return !value.IsEmpty;
        }

        public bool TrySetElementTemperature(Vector2Int pos, short value)
        {
            if (!InsideTheWorldDimensions(pos))
                return false;

            if (Elements[pos.X, pos.Y].Temperature != value)
            {
                TryNotifyChunk(pos);
                Elements[pos.X, pos.Y].SetTemperatureValue(value);
            }

            return true;
        }

        public bool IsEmptyElementSlot(Vector2Int pos)
        {
            if (!InsideTheWorldDimensions(pos) || Elements[pos.X, pos.Y].IsEmpty)
                return true;

            return false;
        }
    }
}
