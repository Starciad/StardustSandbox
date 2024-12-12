using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.World.Data;

using System.Collections.Generic;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld
    {
        public void InstantiateElement<T>(Point position) where T : ISElement
        {
            InstantiateElement(position, this.SGameInstance.ElementDatabase.GetIdOfElementType<T>());
        }
        public void InstantiateElement(Point position, uint identifier)
        {
            InstantiateElement(position, this.SGameInstance.ElementDatabase.GetElementById(identifier));
        }
        public void InstantiateElement(Point position, ISElement value)
        {
            _ = TryInstantiateElement(position, value);
        }
        public bool TryInstantiateElement<T>(Point position) where T : ISElement
        {
            return TryInstantiateElement(position, this.SGameInstance.ElementDatabase.GetIdOfElementType<T>());
        }
        public bool TryInstantiateElement(Point position, uint identifier)
        {
            return TryInstantiateElement(position, this.SGameInstance.ElementDatabase.GetElementById(identifier));
        }
        public bool TryInstantiateElement(Point position, ISElement value)
        {
            if (!InsideTheWorldDimensions(position) || !IsEmptyElementSlot(position))
            {
                return false;
            }

            NotifyChunk(position);

            SWorldSlot worldSlot = this.slots[position.X, position.Y];

            worldSlot.Instantiate(position, value);
            ((SElement)value).InstantiateStep(worldSlot);

            return true;
        }

        public void UpdateElementPosition(Point oldPosition, Point newPosition)
        {
            _ = TryUpdateElementPosition(oldPosition, newPosition);
        }
        public bool TryUpdateElementPosition(Point oldPosition, Point newPosition)
        {
            if (!InsideTheWorldDimensions(oldPosition) ||
                !InsideTheWorldDimensions(newPosition) ||
                 IsEmptyElementSlot(oldPosition) ||
                !IsEmptyElementSlot(newPosition))
            {
                return false;
            }

            NotifyChunk(oldPosition);
            NotifyChunk(newPosition);

            this.slots[newPosition.X, newPosition.Y].Copy(this.slots[oldPosition.X, oldPosition.Y]);
            this.slots[newPosition.X, newPosition.Y].SetPosition(newPosition);
            this.slots[oldPosition.X, oldPosition.Y].Destroy();

            return true;
        }

        public void SwappingElements(Point element1Position, Point element2Position)
        {
            _ = TrySwappingElements(element1Position, element2Position);
        }
        public bool TrySwappingElements(Point element1Position, Point element2Position)
        {
            if (!InsideTheWorldDimensions(element1Position) ||
                !InsideTheWorldDimensions(element2Position) ||
                IsEmptyElementSlot(element1Position) ||
                IsEmptyElementSlot(element2Position))
            {
                return false;
            }

            NotifyChunk(element1Position);
            NotifyChunk(element2Position);

            SWorldSlot tempSlot = this.worldSlotsPool.TryGet(out ISPoolableObject value) ? (SWorldSlot)value : new();

            tempSlot.Copy(this.slots[element1Position.X, element1Position.Y]);

            this.slots[element1Position.X, element1Position.Y].Copy(this.slots[element2Position.X, element2Position.Y]);
            this.slots[element2Position.X, element2Position.Y].Copy(tempSlot);

            this.slots[element1Position.X, element1Position.Y].SetPosition(element1Position);
            this.slots[element2Position.X, element2Position.Y].SetPosition(element2Position);

            this.worldSlotsPool.Add(tempSlot);

            return true;
        }

        public void DestroyElement(Point position)
        {
            _ = TryDestroyElement(position);
        }
        public bool TryDestroyElement(Point position)
        {
            if (!InsideTheWorldDimensions(position) ||
                 IsEmptyElementSlot(position))
            {
                return false;
            }

            NotifyChunk(position);
            this.slots[position.X, position.Y].Destroy();

            return true;
        }

        public void ReplaceElement<T>(Point position) where T : ISElement
        {
            _ = TryReplaceElement<T>(position);
        }
        public void ReplaceElement(Point position, uint identifier)
        {
            _ = TryReplaceElement(position, identifier);
        }
        public void ReplaceElement(Point position, ISElement value)
        {
            _ = TryReplaceElement(position, value);
        }
        public bool TryReplaceElement<T>(Point position) where T : ISElement
        {
            return TryDestroyElement(position) && TryInstantiateElement<T>(position);
        }
        public bool TryReplaceElement(Point position, uint identifier)
        {
            return TryDestroyElement(position) && TryInstantiateElement(position, identifier);
        }
        public bool TryReplaceElement(Point position, ISElement value)
        {
            return TryDestroyElement(position) && TryInstantiateElement(position, value);
        }

        public ISElement GetElement(Point position)
        {
            _ = TryGetElement(position, out ISElement value);
            return value;
        }
        public bool TryGetElement(Point position, out ISElement value)
        {
            if (!InsideTheWorldDimensions(position) ||
                 IsEmptyElementSlot(position))
            {
                value = null;
                return false;
            }

            value = this.slots[position.X, position.Y].Element;
            return true;
        }

        public ISWorldSlot[] GetElementNeighbors(Point position)
        {
            _ = TryGetElementNeighbors(position, out ISWorldSlot[] neighbors);
            return neighbors;
        }
        public bool TryGetElementNeighbors(Point position, out ISWorldSlot[] neighbors)
        {
            neighbors = [];

            if (!InsideTheWorldDimensions(position))
            {
                return false;
            }

            Point[] neighborsPositions = SPointExtensions.GetNeighboringCardinalPoints(position);

            List<ISWorldSlot> slotsFound = [];

            for (int i = 0; i < neighborsPositions.Length; i++)
            {
                if (TryGetElementSlot(position, out ISWorldSlot value))
                {
                    slotsFound.Add(value);
                }
            }

            if (slotsFound.Count > 0)
            {
                neighbors = [.. slotsFound];
                return true;
            }

            return false;
        }

        public ISWorldSlot GetElementSlot(Point position)
        {
            _ = TryGetElementSlot(position, out ISWorldSlot value);
            return value;
        }
        public bool TryGetElementSlot(Point position, out ISWorldSlot value)
        {
            value = default;
            if (!InsideTheWorldDimensions(position) ||
                 IsEmptyElementSlot(position))
            {
                return false;
            }

            value = this.slots[position.X, position.Y];
            return true;
        }

        public void SetElementTemperature(Point position, short value)
        {
            _ = TrySetElementTemperature(position, value);
        }
        public bool TrySetElementTemperature(Point position, short value)
        {
            if (!InsideTheWorldDimensions(position) ||
                 IsEmptyElementSlot(position))
            {
                return false;
            }

            if (this.slots[position.X, position.Y].Temperature != value)
            {
                NotifyChunk(position);
                this.slots[position.X, position.Y].SetTemperatureValue(value);
            }

            return true;
        }

        public void SetElementFreeFalling(Point position, bool value)
        {
            _ = TrySetElementFreeFalling(position, value);
        }

        public bool TrySetElementFreeFalling(Point position, bool value)
        {
            if (!InsideTheWorldDimensions(position) ||
                 IsEmptyElementSlot(position))
            {
                return false;
            }

            this.slots[position.X, position.Y].SetFreeFalling(value);

            return true;
        }

        public void SetElementColor(Point position, Color value)
        {
            _ = TrySetElementColor(position, value);
        }
        public bool TrySetElementColor(Point position, Color value)
        {
            if (!InsideTheWorldDimensions(position) ||
                 IsEmptyElementSlot(position))
            {
                return false;
            }

            this.slots[position.X, position.Y].SetColor(value);

            return true;
        }

        // Tools
        public bool IsEmptyElementSlot(Point position)
        {
            return !InsideTheWorldDimensions(position) || this.slots[position.X, position.Y].IsEmpty;
        }
    }
}
