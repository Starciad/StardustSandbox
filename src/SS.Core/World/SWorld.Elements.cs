using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.World.Data;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld
    {
        public void InstantiateElement<T>(SWorldLayer worldLayer, Point position) where T : ISElement
        {
            InstantiateElement(worldLayer, position, this.SGameInstance.ElementDatabase.GetIdOfElementType<T>());
        }
        public void InstantiateElement(SWorldLayer worldLayer, Point position, uint identifier)
        {
            InstantiateElement(worldLayer, position, this.SGameInstance.ElementDatabase.GetElementById(identifier));
        }
        public void InstantiateElement(SWorldLayer worldLayer, Point position, ISElement value)
        {
            _ = TryInstantiateElement(worldLayer, position, value);
        }
        public bool TryInstantiateElement<T>(SWorldLayer worldLayer, Point position) where T : ISElement
        {
            return TryInstantiateElement(worldLayer, position, this.SGameInstance.ElementDatabase.GetIdOfElementType<T>());
        }
        public bool TryInstantiateElement(SWorldLayer worldLayer, Point position, uint identifier)
        {
            return TryInstantiateElement(worldLayer, position, this.SGameInstance.ElementDatabase.GetElementById(identifier));
        }
        public bool TryInstantiateElement(SWorldLayer worldLayer, Point position, ISElement value)
        {
            if (!InsideTheWorldDimensions(position) ||
                !IsEmptyElementSlot(position))
            {
                return false;
            }

            NotifyChunk(position);

            SWorldSlot worldSlot = this.slots[position.X, position.Y];

            worldSlot.Instantiate(worldLayer, position, value);
            ((SElement)value).InstantiateStep(worldSlot);

            return true;
        }

        public void UpdateElementPosition(SWorldLayer worldLayer, Point oldPosition, Point newPosition)
        {
            _ = TryUpdateElementPosition(worldLayer, oldPosition, newPosition);
        }
        public bool TryUpdateElementPosition(SWorldLayer worldLayer, Point oldPosition, Point newPosition)
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

            this.slots[newPosition.X, newPosition.Y].Copy(worldLayer, this.slots[oldPosition.X, oldPosition.Y].GetLayer(worldLayer));
            this.slots[newPosition.X, newPosition.Y].SetPosition(newPosition);
            this.slots[oldPosition.X, oldPosition.Y].Destroy(worldLayer);

            return true;
        }

        public void SwappingElements(SWorldLayer worldLayer, Point element1Position, Point element2Position)
        {
            _ = TrySwappingElements(worldLayer, element1Position, element2Position);
        }
        public bool TrySwappingElements(SWorldLayer worldLayer, Point element1Position, Point element2Position)
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

            tempSlot.Copy(worldLayer, this.slots[element1Position.X, element1Position.Y].GetLayer(worldLayer));

            this.slots[element1Position.X, element1Position.Y].Copy(worldLayer, this.slots[element2Position.X, element2Position.Y].GetLayer(worldLayer));
            this.slots[element2Position.X, element2Position.Y].Copy(worldLayer, tempSlot.GetLayer(worldLayer));

            this.slots[element1Position.X, element1Position.Y].SetPosition(element1Position);
            this.slots[element2Position.X, element2Position.Y].SetPosition(element2Position);

            this.worldSlotsPool.Add(tempSlot);

            return true;
        }

        public void DestroyElement(SWorldLayer worldLayer, Point position)
        {
            _ = TryDestroyElement(worldLayer, position);
        }
        public bool TryDestroyElement(SWorldLayer worldLayer, Point position)
        {
            if (!InsideTheWorldDimensions(position) ||
                IsEmptyElementSlot(position))
            {
                return false;
            }

            NotifyChunk(position);
            this.slots[position.X, position.Y].Destroy(worldLayer);

            return true;
        }

        public void ReplaceElement<T>(SWorldLayer worldLayer, Point position) where T : ISElement
        {
            _ = TryReplaceElement<T>(worldLayer, position);
        }
        public void ReplaceElement(SWorldLayer worldLayer, Point position, uint identifier)
        {
            _ = TryReplaceElement(worldLayer, position, identifier);
        }
        public void ReplaceElement(SWorldLayer worldLayer, Point position, ISElement value)
        {
            _ = TryReplaceElement(worldLayer, position, value);
        }
        public bool TryReplaceElement<T>(SWorldLayer worldLayer, Point position) where T : ISElement
        {
            return TryDestroyElement(worldLayer, position) && TryInstantiateElement<T>(worldLayer, position);
        }
        public bool TryReplaceElement(SWorldLayer worldLayer, Point position, uint identifier)
        {
            return TryDestroyElement(worldLayer, position) && TryInstantiateElement(worldLayer, position, identifier);
        }
        public bool TryReplaceElement(SWorldLayer worldLayer, Point position, ISElement value)
        {
            return TryDestroyElement(worldLayer, position) && TryInstantiateElement(worldLayer, position, value);
        }

        public ISElement GetElement(SWorldLayer worldLayer, Point position)
        {
            _ = TryGetElement(worldLayer, position, out ISElement value);
            return value;
        }
        public bool TryGetElement(SWorldLayer worldLayer, Point position, out ISElement value)
        {
            if (!InsideTheWorldDimensions(position) ||
                IsEmptyElementSlot(position))
            {
                value = null;
                return false;
            }

            value = this.slots[position.X, position.Y].GetLayer(worldLayer).Element;
            return true;
        }

        public ReadOnlySpan<ISWorldSlot> GetNeighboringSlots(Point position)
        {
            _ = TryGetNeighboringSlots(position, out ISWorldSlot[] neighbors);
            return neighbors;
        }
        public bool TryGetNeighboringSlots(Point position, out ISWorldSlot[] neighbors)
        {
            neighbors = [];

            if (!InsideTheWorldDimensions(position))
            {
                return false;
            }

            List<ISWorldSlot> slotsFound = [];

            foreach (Point neighborPosition in SPointExtensions.GetNeighboringCardinalPoints(position))
            {
                if (TryGetElementSlot(neighborPosition, out ISWorldSlot value))
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

        public void SetElementTemperature(SWorldLayer worldLayer, Point position, short value)
        {
            _ = TrySetElementTemperature(worldLayer, position, value);
        }
        public bool TrySetElementTemperature(SWorldLayer worldLayer, Point position, short value)
        {
            if (!InsideTheWorldDimensions(position) ||
                IsEmptyElementSlot(position))
            {
                return false;
            }

            SWorldSlotLayer layer = this.slots[position.X, position.Y].GetLayer(worldLayer);

            if (layer.Temperature != value)
            {
                NotifyChunk(position);
                layer.SetTemperatureValue(value);
            }

            return true;
        }

        public void SetElementFreeFalling(SWorldLayer worldLayer, Point position, bool value)
        {
            _ = TrySetElementFreeFalling(worldLayer, position, value);
        }

        public bool TrySetElementFreeFalling(SWorldLayer worldLayer, Point position, bool value)
        {
            if (!InsideTheWorldDimensions(position) ||
                IsEmptyElementSlot(position))
            {
                return false;
            }

            this.slots[position.X, position.Y].GetLayer(worldLayer).SetFreeFalling(value);

            return true;
        }

        public void SetElementColorModifier(SWorldLayer worldLayer, Point position, Color value)
        {
            _ = TrySetElementColorModifier(worldLayer, position, value);
        }
        public bool TrySetElementColorModifier(SWorldLayer worldLayer, Point position, Color value)
        {
            if (!InsideTheWorldDimensions(position) ||
                IsEmptyElementSlot(position))
            {
                return false;
            }

            this.slots[position.X, position.Y].GetLayer(worldLayer).SetColorModifier(value);

            return true;
        }

        // Tools
        public bool IsEmptyElementSlot(Point position)
        {
            return !InsideTheWorldDimensions(position) || this.slots[position.X, position.Y].IsEmpty;
        }
    }
}
