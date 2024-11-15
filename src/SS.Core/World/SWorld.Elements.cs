using Microsoft.Xna.Framework;

using StardustSandbox.Game.Interfaces.Elements;
using StardustSandbox.Game.Interfaces.General;
using StardustSandbox.Game.Interfaces.World;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.World
{
    public sealed partial class SWorld : ISElementManager
    {
        public void InstantiateElement<T>(Point pos) where T : ISElement
        {
            InstantiateElement(pos, this.SGameInstance.ElementDatabase.GetIdOfElementType<T>());
        }
        public void InstantiateElement(Point pos, uint id)
        {
            InstantiateElement(pos, this.SGameInstance.ElementDatabase.GetElementById(id));
        }
        public void InstantiateElement(Point pos, ISElement value)
        {
            _ = TryInstantiateElement(pos, value);
        }
        public bool TryInstantiateElement<T>(Point pos) where T : ISElement
        {
            return TryInstantiateElement(pos, this.SGameInstance.ElementDatabase.GetIdOfElementType<T>());
        }
        public bool TryInstantiateElement(Point pos, uint id)
        {
            return TryInstantiateElement(pos, this.SGameInstance.ElementDatabase.GetElementById(id));
        }
        public bool TryInstantiateElement(Point pos, ISElement value)
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

            this.slots[newPos.X, newPos.Y].Copy(this.slots[oldPos.X, oldPos.Y]);
            this.slots[oldPos.X, oldPos.Y].Destroy();
            return true;
        }

        public void SwappingElements(Point element1Pos, Point element2Pos)
        {
            _ = TrySwappingElements(element1Pos, element2Pos);
        }
        public bool TrySwappingElements(Point element1Pos, Point element2Pos)
        {
            if (!InsideTheWorldDimensions(element1Pos) ||
                !InsideTheWorldDimensions(element2Pos) ||
                IsEmptyElementSlot(element1Pos) ||
                IsEmptyElementSlot(element2Pos))
            {
                return false;
            }

            NotifyChunk(element1Pos);
            NotifyChunk(element2Pos);

            SWorldSlot tempSlot = this.worldSlotsPool.TryGet(out ISPoolableObject value) ? (SWorldSlot)value : new();

            tempSlot.Copy(this.slots[element1Pos.X, element1Pos.Y]);

            this.slots[element1Pos.X, element1Pos.Y].Copy(this.slots[element2Pos.X, element2Pos.Y]);
            this.slots[element2Pos.X, element2Pos.Y].Copy(tempSlot);

            this.worldSlotsPool.Add(tempSlot);

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

        public void ReplaceElement<T>(Point pos) where T : ISElement
        {
            _ = TryReplaceElement<T>(pos);
        }
        public void ReplaceElement(Point pos, uint id)
        {
            _ = TryReplaceElement(pos, id);
        }
        public void ReplaceElement(Point pos, ISElement value)
        {
            _ = TryReplaceElement(pos, value);
        }
        public bool TryReplaceElement<T>(Point pos) where T : ISElement
        {
            return TryDestroyElement(pos) && TryInstantiateElement<T>(pos);
        }
        public bool TryReplaceElement(Point pos, uint id)
        {
            return TryDestroyElement(pos) && TryInstantiateElement(pos, id);
        }
        public bool TryReplaceElement(Point pos, ISElement value)
        {
            return TryDestroyElement(pos) && TryInstantiateElement(pos, value);
        }

        public ISElement GetElement(Point pos)
        {
            _ = TryGetElement(pos, out ISElement value);
            return value;
        }
        public bool TryGetElement(Point pos, out ISElement value)
        {
            if (!InsideTheWorldDimensions(pos) ||
                 IsEmptyElementSlot(pos))
            {
                value = null;
                return false;
            }

            value = this.slots[pos.X, pos.Y].Element;
            return true;
        }

        public ReadOnlySpan<(Point, ISWorldSlot)> GetElementNeighbors(Point pos)
        {
            _ = TryGetElementNeighbors(pos, out ReadOnlySpan<(Point, ISWorldSlot)> neighbors);
            return neighbors;
        }
        public bool TryGetElementNeighbors(Point pos, out ReadOnlySpan<(Point, ISWorldSlot)> neighbors)
        {
            neighbors = default;

            if (!InsideTheWorldDimensions(pos))
            {
                return false;
            }

            Point[] neighborsPositions = GetElementNeighborPositions(pos);

            (Point, ISWorldSlot)[] slotsFound = new (Point, ISWorldSlot)[neighborsPositions.Length];
            int count = 0;

            for (int i = 0; i < neighborsPositions.Length; i++)
            {
                Point position = neighborsPositions[i];
                if (TryGetElementSlot(position, out ISWorldSlot value))
                {
                    slotsFound[count] = (position, value);
                    count++;
                }
            }

            if (count > 0)
            {
                neighbors = new ReadOnlySpan<(Point, ISWorldSlot)>(slotsFound, 0, count);
                return true;
            }

            return false;
        }

        public ISWorldSlot GetElementSlot(Point pos)
        {
            _ = TryGetElementSlot(pos, out ISWorldSlot value);
            return value;
        }
        public bool TryGetElementSlot(Point pos, out ISWorldSlot value)
        {
            value = default;
            if (!InsideTheWorldDimensions(pos) ||
                 IsEmptyElementSlot(pos))
            {
                return false;
            }

            value = this.slots[pos.X, pos.Y];
            return true;
        }

        public void SetElementTemperature(Point pos, short value)
        {
            _ = TrySetElementTemperature(pos, value);
        }
        public bool TrySetElementTemperature(Point pos, short value)
        {
            if (!InsideTheWorldDimensions(pos) ||
                 IsEmptyElementSlot(pos))
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

        public void SetElementFreeFalling(Point pos, bool value)
        {
            _ = TrySetElementFreeFalling(pos, value);
        }

        public bool TrySetElementFreeFalling(Point pos, bool value)
        {
            if (!InsideTheWorldDimensions(pos) ||
                 IsEmptyElementSlot(pos))
            {
                return false;
            }

            this.slots[pos.X, pos.Y].SetFreeFalling(value);

            return true;
        }

        public void SetElementColor(Point pos, Color value)
        {
            _ = TrySetElementColor(pos, value);
        }
        public bool TrySetElementColor(Point pos, Color value)
        {
            if (!InsideTheWorldDimensions(pos) ||
                 IsEmptyElementSlot(pos))
            {
                return false;
            }

            this.slots[pos.X, pos.Y].SetColor(value);

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
