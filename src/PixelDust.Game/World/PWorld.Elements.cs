using Microsoft.Xna.Framework;

using PixelDust.Game.Elements;
using PixelDust.Game.Interfaces.Elements;
using PixelDust.Game.World.Data;

using System;

namespace PixelDust.Game.World
{
    public sealed partial class PWorld : IPElementManager
    {
        public void InstantiateElement<T>(Point pos) where T : PElement
        {
            InstantiateElement(pos, elementDatabase.GetIdOfElementType<T>());
        }
        public void InstantiateElement(Point pos, uint id)
        {
            InstantiateElement(pos, elementDatabase.GetElementById(id));
        }
        public void InstantiateElement(Point pos, PElement value)
        {
            _ = TryInstantiateElement(pos, value);
        }
        public bool TryInstantiateElement<T>(Point pos) where T : PElement
        {
            return TryInstantiateElement(pos, elementDatabase.GetIdOfElementType<T>());
        }
        public bool TryInstantiateElement(Point pos, uint id)
        {
            return TryInstantiateElement(pos, elementDatabase.GetElementById(id));
        }
        public bool TryInstantiateElement(Point pos, PElement value)
        {
            if (!InsideTheWorldDimensions(pos) || !IsEmptyElementSlot(pos))
            {
                return false;
            }

            NotifyChunk(pos);

            this.slots[pos.X, pos.Y].Instantiate(value);
            return true;
        }

        public void UpdateElementPosition(Point oldPos, Point newPos)
        {
            _ = TryUpdateElementPosition(oldPos, newPos);
        }
        public bool TryUpdateElementPosition(Point oldPos, Point newPos)
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

        public void SwappingElements(Point element1, Point element2)
        {
            _ = TrySwappingElements(element1, element2);
        }
        public bool TrySwappingElements(Point element1, Point element2)
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

        public void DestroyElement(Point pos)
        {
            _ = TryDestroyElement(pos);
        }
        public bool TryDestroyElement(Point pos)
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

        public void ReplaceElement<T>(Point pos) where T : PElement
        {
            _ = TryReplaceElement<T>(pos);
        }
        public void ReplaceElement(Point pos, uint id)
        {
            _ = TryReplaceElement(pos, id);
        }
        public void ReplaceElement(Point pos, PElement value)
        {
            _ = TryReplaceElement(pos, value);
        }
        public bool TryReplaceElement<T>(Point pos) where T : PElement
        {
            return TryDestroyElement(pos) && TryInstantiateElement<T>(pos);
        }
        public bool TryReplaceElement(Point pos, uint id)
        {
            return TryDestroyElement(pos) && TryInstantiateElement(pos, id);
        }
        public bool TryReplaceElement(Point pos, PElement value)
        {
            return TryDestroyElement(pos) && TryInstantiateElement(pos, value);
        }

        public PElement GetElement(Point pos)
        {
            _ = TryGetElement(pos, out PElement value);
            return value;
        }
        public bool TryGetElement(Point pos, out PElement value)
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

        public ReadOnlySpan<(Point, PWorldSlot)> GetElementNeighbors(Point pos)
        {
            _ = TryGetElementNeighbors(pos, out ReadOnlySpan<(Point, PWorldSlot)> neighbors);
            return neighbors;
        }
        public bool TryGetElementNeighbors(Point pos, out ReadOnlySpan<(Point, PWorldSlot)> neighbors)
        {
            neighbors = default;

            if (!InsideTheWorldDimensions(pos))
            {
                return false;
            }

            Point[] neighborsPositions = GetElementNeighborPositions(pos);

            (Point, PWorldSlot)[] slotsFound = new (Point, PWorldSlot)[neighborsPositions.Length];
            int count = 0;

            for (int i = 0; i < neighborsPositions.Length; i++)
            {
                Point position = neighborsPositions[i];
                if (TryGetElementSlot(position, out PWorldSlot value))
                {
                    slotsFound[count] = (position, value);
                    count++;
                }
            }

            if (count > 0)
            {
                neighbors = new ReadOnlySpan<(Point, PWorldSlot)>(slotsFound, 0, count);
                return true;
            }

            return false;
        }

        public PWorldSlot GetElementSlot(Point pos)
        {
            _ = TryGetElementSlot(pos, out PWorldSlot value);
            return value;
        }
        public bool TryGetElementSlot(Point pos, out PWorldSlot value)
        {
            value = default;
            if (!InsideTheWorldDimensions(pos))
            {
                return false;
            }

            value = this.slots[pos.X, pos.Y];
            return !value.IsEmpty;
        }

        public void SetElementTemperature(Point pos, short value)
        {
            _ = TrySetElementTemperature(pos, value);
        }
        public bool TrySetElementTemperature(Point pos, short value)
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
        public bool IsEmptyElementSlot(Point pos)
        {
            return !InsideTheWorldDimensions(pos) || this.slots[pos.X, pos.Y].IsEmpty;
        }

        // Utilities
        private static Point[] GetElementNeighborPositions(Point pos)
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

        public bool InsideTheWorldDimensions(Point pos)
        {
            return pos.X >= 0 && pos.X < this.Infos.Size.Width &&
                   pos.Y >= 0 && pos.Y < this.Infos.Size.Height;
        }
    }
}
