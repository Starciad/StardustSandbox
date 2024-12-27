using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.World.Data;

using System.Collections.Generic;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld
    {
        public void InstantiateElement(Point position, SWorldLayer worldLayer, string identifier)
        {
            InstantiateElement(position, worldLayer, this.SGameInstance.ElementDatabase.GetElementByIdentifier(identifier));
        }
        public void InstantiateElement(Point position, SWorldLayer worldLayer, ISElement value)
        {
            _ = TryInstantiateElement(position, worldLayer, value);
        }
        public bool TryInstantiateElement(Point position, SWorldLayer worldLayer, string identifier)
        {
            return TryInstantiateElement(position, worldLayer, this.SGameInstance.ElementDatabase.GetElementByIdentifier(identifier));
        }
        public bool TryInstantiateElement(Point position, SWorldLayer worldLayer, ISElement value)
        {
            if (!InsideTheWorldDimensions(position) || !IsEmptyWorldSlotLayer(position, worldLayer))
            {
                return false;
            }

            NotifyChunk(position);

            SWorldSlot worldSlot = this.slots[position.X, position.Y];

            worldSlot.Instantiate(position, worldLayer, value);

            this.worldElementContext.UpdateInformation(position, worldLayer, worldSlot);

            value.Context = this.worldElementContext;
            value.InstantiateStep(worldSlot, worldLayer);

            return true;
        }

        public void UpdateElementPosition(Point oldPosition, Point newPosition, SWorldLayer worldLayer)
        {
            _ = TryUpdateElementPosition(oldPosition, newPosition, worldLayer);
        }
        public bool TryUpdateElementPosition(Point oldPosition, Point newPosition, SWorldLayer worldLayer)
        {
            if (!InsideTheWorldDimensions(oldPosition) ||
                !InsideTheWorldDimensions(newPosition) ||
                 IsEmptyWorldSlotLayer(oldPosition, worldLayer) ||
                !IsEmptyWorldSlotLayer(newPosition, worldLayer))
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

        public void SwappingElements(Point element1Position, Point element2Position, SWorldLayer worldLayer)
        {
            _ = TrySwappingElements(element1Position, element2Position, worldLayer);
        }
        public bool TrySwappingElements(Point element1Position, Point element2Position, SWorldLayer worldLayer)
        {
            if (!InsideTheWorldDimensions(element1Position) ||
                !InsideTheWorldDimensions(element2Position) ||
                IsEmptyWorldSlotLayer(element1Position, worldLayer) ||
                IsEmptyWorldSlotLayer(element2Position, worldLayer))
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

        public void DestroyElement(Point position, SWorldLayer worldLayer)
        {
            _ = TryDestroyElement(position, worldLayer);
        }
        public bool TryDestroyElement(Point position, SWorldLayer worldLayer)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptyWorldSlotLayer(position, worldLayer))
            {
                return false;
            }

            NotifyChunk(position);
            this.slots[position.X, position.Y].Destroy(worldLayer);

            return true;
        }

        public void ReplaceElement(Point position, SWorldLayer worldLayer, string identifier)
        {
            _ = TryReplaceElement(position, worldLayer, identifier);
        }
        public void ReplaceElement(Point position, SWorldLayer worldLayer, ISElement value)
        {
            _ = TryReplaceElement(position, worldLayer, value);
        }
        public bool TryReplaceElement(Point position, SWorldLayer worldLayer, string identifier)
        {
            return TryDestroyElement(position, worldLayer) && TryInstantiateElement(position, worldLayer, identifier);
        }
        public bool TryReplaceElement(Point position, SWorldLayer worldLayer, ISElement value)
        {
            return TryDestroyElement(position, worldLayer) && TryInstantiateElement(position, worldLayer, value);
        }

        public ISElement GetElement(Point position, SWorldLayer worldLayer)
        {
            _ = TryGetElement(position, worldLayer, out ISElement value);
            return value;
        }
        public bool TryGetElement(Point position, SWorldLayer worldLayer, out ISElement value)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptyWorldSlotLayer(position, worldLayer))
            {
                value = null;
                return false;
            }

            SWorldSlotLayer worldSlotLayer = this.slots[position.X, position.Y].GetLayer(worldLayer);

            if (worldSlotLayer.IsEmpty)
            {
                value = null;
                return false;
            }

            value = worldSlotLayer.Element;
            return true;
        }

        public IEnumerable<SWorldSlot> GetNeighboringSlots(Point position)
        {
            foreach (Point neighborPosition in SPointExtensions.GetNeighboringCardinalPoints(position))
            {
                if (TryGetWorldSlot(neighborPosition, out SWorldSlot value))
                {
                    yield return value;
                }
            }
        }

        public SWorldSlot GetWorldSlot(Point position)
        {
            _ = TryGetWorldSlot(position, out SWorldSlot value);
            return value;
        }
        public bool TryGetWorldSlot(Point position, out SWorldSlot value)
        {
            value = default;

            if (!InsideTheWorldDimensions(position) || IsEmptyWorldSlot(position))
            {
                return false;
            }

            value = this.slots[position.X, position.Y];
            return true;
        }

        public void SetElementTemperature(Point position, SWorldLayer worldLayer, short value)
        {
            _ = TrySetElementTemperature(position, worldLayer, value);
        }
        public bool TrySetElementTemperature(Point position, SWorldLayer worldLayer, short value)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptyWorldSlotLayer(position, worldLayer))
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

        public void SetElementFreeFalling(Point position, SWorldLayer worldLayer, bool value)
        {
            _ = TrySetElementFreeFalling(position, worldLayer, value);
        }

        public bool TrySetElementFreeFalling(Point position, SWorldLayer worldLayer, bool value)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptyWorldSlotLayer(position, worldLayer))
            {
                return false;
            }

            this.slots[position.X, position.Y].GetLayer(worldLayer).SetFreeFalling(value);

            return true;
        }

        public void SetElementColorModifier(Point position, SWorldLayer worldLayer, Color value)
        {
            _ = TrySetElementColorModifier(position, worldLayer, value);
        }
        public bool TrySetElementColorModifier(Point position, SWorldLayer worldLayer, Color value)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptyWorldSlotLayer(position, worldLayer))
            {
                return false;
            }

            this.slots[position.X, position.Y].GetLayer(worldLayer).SetColorModifier(value);

            return true;
        }

        // Tools
        public bool IsEmptyWorldSlot(Point position)
        {
            return !InsideTheWorldDimensions(position) || this.slots[position.X, position.Y].IsEmpty;
        }

        public bool IsEmptyWorldSlotLayer(Point position, SWorldLayer worldLayer)
        {
            return !InsideTheWorldDimensions(position) || this.slots[position.X, position.Y].GetLayer(worldLayer).IsEmpty;
        }
    }
}
