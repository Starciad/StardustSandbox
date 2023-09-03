using PixelDust.Core.Elements;
using PixelDust.Core.Mathematics;

using System;
using System.Collections.Generic;

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

        public bool TryGetElementNeighbors(Vector2Int pos, out (Vector2Int, PWorldElementSlot)[] neighbors)
        {
            List<(Vector2Int, PWorldElementSlot)> slotsFound = new();
            neighbors = Array.Empty<(Vector2Int, PWorldElementSlot)>();

            if (!InsideTheWorldDimensions(pos))
                return false;

            Vector2Int[] neighborsPositions = new Vector2Int[]
            {
                // Top
                new(pos.X, pos.Y - 1),
                new(pos.X + 1, pos.Y - 1),
                new(pos.X - 1, pos.Y - 1),

                // Center
                new(pos.X + 1, pos.Y),
                new(pos.X - 1, pos.Y),

                // Down
                new(pos.X, pos.Y + 1),
                new(pos.X + 1, pos.Y + 1),
                new(pos.X - 1, pos.Y + 1),
            };

            foreach (Vector2Int neighborPos in neighborsPositions)
            {
                if (TryGetElementSlot(neighborPos, out PWorldElementSlot value))
                    slotsFound.Add((neighborPos, value));
            }

            if (slotsFound.Count > 0)
            {
                neighbors = slotsFound.ToArray();
                return true;
            }

            return false;
        }

        public bool TryGetElementSlot(Vector2Int pos, out PWorldElementSlot value)
        {
            value = default;
            if (!InsideTheWorldDimensions(pos))
                return false;

            value = Elements[pos.X, pos.Y];
            return !value.IsEmpty;
        }

        public bool IsEmptyElementSlot(Vector2Int pos)
        {
            if (!InsideTheWorldDimensions(pos) || Elements[pos.X, pos.Y].IsEmpty)
                return true;

            return false;
        }
    }
}
