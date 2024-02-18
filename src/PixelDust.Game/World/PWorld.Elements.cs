using PixelDust.Game.Elements;
using PixelDust.Game.Elements.Interfaces;
using PixelDust.Game.Mathematics;
using PixelDust.Game.World.Data;

using System;

namespace PixelDust.Game.World
{
    public sealed partial class PWorld : IElementManager
    {
        public void InstantiateElement<T>(Vector2Int pos) where T : PElement
        {
            InstantiateElement(pos, elementDatabase.GetIdOfElementType<T>());
        }
        public void InstantiateElement(Vector2Int pos, uint id)
        {
            InstantiateElement(pos, elementDatabase.GetElementById(id));
        }
        public void InstantiateElement(Vector2Int pos, PElement value)
        {
            _ = TryInstantiateElement(pos, value);
        }
        public bool TryInstantiateElement<T>(Vector2Int pos) where T : PElement
        {
            return TryInstantiateElement(pos, elementDatabase.GetIdOfElementType<T>());
        }
        public bool TryInstantiateElement(Vector2Int pos, uint id)
        {
            return TryInstantiateElement(pos, elementDatabase.GetElementById(id));
        }
        public bool TryInstantiateElement(Vector2Int pos, PElement value)
        {
            if (!InsideTheWorldDimensions(pos) || !IsEmptyElementSlot(pos))
            {
                return false;
            }

            NotifyChunk(pos);

            this.slots[pos.X, pos.Y].Instantiate(value);
            return true;
        }

        public void UpdateElementPosition(Vector2Int oldPos, Vector2Int newPos)
        {
            _ = TryUpdateElementPosition(oldPos, newPos);
        }
        public bool TryUpdateElementPosition(Vector2Int oldPos, Vector2Int newPos)
        {
            if (!InsideTheWorldDimensions(oldPos) ||
                !InsideTheWorldDimensions(newPos) ||
                IsEmptyElementSlot(oldPos) ||
                !IsEmptyElementSlot(newPos))
            {
                return false;
            }

            NotifyChunk(oldPos);
            NotifyChunk(newPos);

            this.slots[newPos.X, newPos.Y] = (PWorldSlot)this.slots[oldPos.X, oldPos.Y].Clone();
            this.slots[oldPos.X, oldPos.Y].Destroy();
            return true;
        }

        public void SwappingElements(Vector2Int element1, Vector2Int element2)
        {
            _ = TrySwappingElements(element1, element2);
        }
        public bool TrySwappingElements(Vector2Int element1, Vector2Int element2)
        {
            if (!InsideTheWorldDimensions(element1) ||
                !InsideTheWorldDimensions(element2) ||
                IsEmptyElementSlot(element1) ||
                IsEmptyElementSlot(element2))
            {
                return false;
            }

            NotifyChunk(element1);
            NotifyChunk(element2);

            PWorldSlot oldValue = (PWorldSlot)this.slots[element1.X, element1.Y].Clone();
            PWorldSlot newValue = (PWorldSlot)this.slots[element2.X, element2.Y].Clone();

            this.slots[element1.X, element1.Y] = newValue;
            this.slots[element2.X, element2.Y] = oldValue;

            return true;
        }

        public void DestroyElement(Vector2Int pos)
        {
            _ = TryDestroyElement(pos);
        }
        public bool TryDestroyElement(Vector2Int pos)
        {
            if (!InsideTheWorldDimensions(pos) ||
                IsEmptyElementSlot(pos))
            {
                return false;
            }

            NotifyChunk(pos);
            this.slots[pos.X, pos.Y].Destroy();

            return true;
        }

        public void ReplaceElement<T>(Vector2Int pos) where T : PElement
        {
            _ = TryReplaceElement<T>(pos);
        }
        public void ReplaceElement(Vector2Int pos, uint id)
        {
            _ = TryReplaceElement(pos, id);
        }
        public void ReplaceElement(Vector2Int pos, PElement value)
        {
            _ = TryReplaceElement(pos, value);
        }
        public bool TryReplaceElement<T>(Vector2Int pos) where T : PElement
        {
            return TryDestroyElement(pos) && TryInstantiateElement<T>(pos);
        }
        public bool TryReplaceElement(Vector2Int pos, uint id)
        {
            return TryDestroyElement(pos) && TryInstantiateElement(pos, id);
        }
        public bool TryReplaceElement(Vector2Int pos, PElement value)
        {
            return TryDestroyElement(pos) && TryInstantiateElement(pos, value);
        }

        public PElement GetElement(Vector2Int pos)
        {
            _ = TryGetElement(pos, out PElement value);
            return value;
        }
        public bool TryGetElement(Vector2Int pos, out PElement value)
        {
            if (!InsideTheWorldDimensions(pos) ||
                IsEmptyElementSlot(pos))
            {
                value = null;
                return false;
            }

            value = elementDatabase.GetElementById(this.slots[pos.X, pos.Y].Id);
            return true;
        }

        public ReadOnlySpan<(Vector2Int, PWorldSlot)> GetElementNeighbors(Vector2Int pos)
        {
            _ = TryGetElementNeighbors(pos, out ReadOnlySpan<(Vector2Int, PWorldSlot)> neighbors);
            return neighbors;
        }
        public bool TryGetElementNeighbors(Vector2Int pos, out ReadOnlySpan<(Vector2Int, PWorldSlot)> neighbors)
        {
            neighbors = default;

            if (!InsideTheWorldDimensions(pos))
            {
                return false;
            }

            Vector2Int[] neighborsPositions = GetElementNeighborPositions(pos);

            (Vector2Int, PWorldSlot)[] slotsFound = new (Vector2Int, PWorldSlot)[neighborsPositions.Length];
            int count = 0;

            for (int i = 0; i < neighborsPositions.Length; i++)
            {
                Vector2Int position = neighborsPositions[i];
                if (TryGetElementSlot(position, out PWorldSlot value))
                {
                    slotsFound[count] = (position, value);
                    count++;
                }
            }

            if (count > 0)
            {
                neighbors = new ReadOnlySpan<(Vector2Int, PWorldSlot)>(slotsFound, 0, count);
                return true;
            }

            return false;
        }

        public PWorldSlot GetElementSlot(Vector2Int pos)
        {
            _ = TryGetElementSlot(pos, out PWorldSlot value);
            return value;
        }
        public bool TryGetElementSlot(Vector2Int pos, out PWorldSlot value)
        {
            value = default;
            if (!InsideTheWorldDimensions(pos))
            {
                return false;
            }

            value = this.slots[pos.X, pos.Y];
            return !value.IsEmpty;
        }

        public void SetElementTemperature(Vector2Int pos, short value)
        {
            _ = TrySetElementTemperature(pos, value);
        }
        public bool TrySetElementTemperature(Vector2Int pos, short value)
        {
            if (!InsideTheWorldDimensions(pos))
            {
                return false;
            }

            if (this.slots[pos.X, pos.Y].Temperature != value)
            {
                NotifyChunk(pos);
                this.slots[pos.X, pos.Y].SetTemperatureValue(value);
            }

            return true;
        }

        // Tools
        public bool IsEmptyElementSlot(Vector2Int pos)
        {
            return !InsideTheWorldDimensions(pos) || this.slots[pos.X, pos.Y].IsEmpty;
        }

        // Utilities
        private static Vector2Int[] GetElementNeighborPositions(Vector2Int pos)
        {
            return
            [
                new(pos.X, pos.Y - 1),
                new(pos.X + 1, pos.Y - 1),
                new(pos.X - 1, pos.Y - 1),
                new(pos.X + 1, pos.Y),
                new(pos.X - 1, pos.Y),
                new(pos.X, pos.Y + 1),
                new(pos.X + 1, pos.Y + 1),
                new(pos.X - 1, pos.Y + 1),
            ];
        }

        public bool InsideTheWorldDimensions(Vector2Int pos)
        {
            return pos.X >= 0 && pos.X < this.Infos.Size.Width &&
                   pos.Y >= 0 && pos.Y < this.Infos.Size.Height;
        }
    }
}
