using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.Core.Interfaces.Elements
{
    public interface ISElementContext : ISElementManager
    {
        ISWorldSlot Slot { get; }
        ISWorldSlotLayer SlotLayer { get; }
        SWorldLayer Layer { get; }

        void SetPosition(Point newPosition);
        void SetPosition(Point newPosition, SWorldLayer worldLayer);
        bool TrySetPosition(Point newPosition);
        bool TrySetPosition(Point newPosition, SWorldLayer worldLayer);

        void InstantiateElement<T>() where T : ISElement;
        void InstantiateElement(uint identifier);
        void InstantiateElement(ISElement value);
        void InstantiateElement<T>(SWorldLayer worldLayer) where T : ISElement;
        void InstantiateElement(SWorldLayer worldLayer, uint identifier);
        void InstantiateElement(SWorldLayer worldLayer, ISElement value);
        bool TryInstantiateElement<T>() where T : ISElement;
        bool TryInstantiateElement(uint identifier);
        bool TryInstantiateElement(ISElement value);
        bool TryInstantiateElement<T>(SWorldLayer worldLayer) where T : ISElement;
        bool TryInstantiateElement(SWorldLayer worldLayer, uint identifier);
        bool TryInstantiateElement(SWorldLayer worldLayer, ISElement value);

        void UpdateElementPosition(Point newPosition);
        void UpdateElementPosition(Point newPosition, SWorldLayer worldLayer);
        bool TryUpdateElementPosition(Point newPosition);
        bool TryUpdateElementPosition(Point newPosition, SWorldLayer worldLayer);

        void SwappingElements(Point targetPosition);
        void SwappingElements(Point targetPosition, SWorldLayer worldLayer);
        bool TrySwappingElements(Point targetPosition);
        bool TrySwappingElements(Point targetPosition, SWorldLayer worldLayer);

        void DestroyElement();
        void DestroyElement(SWorldLayer worldLayer);
        bool TryDestroyElement();
        bool TryDestroyElement(SWorldLayer worldLayer);

        void ReplaceElement<T>() where T : ISElement;
        void ReplaceElement<T>(SWorldLayer worldLayer) where T : ISElement;
        void ReplaceElement(uint identifier);
        void ReplaceElement(SWorldLayer worldLayer, uint identifier);
        void ReplaceElement(ISElement value);
        void ReplaceElement(SWorldLayer worldLayer, ISElement value);
        bool TryReplaceElement<T>() where T : ISElement;
        bool TryReplaceElement(uint identifier);
        bool TryReplaceElement(ISElement value);
        bool TryReplaceElement<T>(SWorldLayer worldLayer) where T : ISElement;
        bool TryReplaceElement(SWorldLayer worldLayer, uint identifier);
        bool TryReplaceElement(SWorldLayer worldLayer, ISElement value);

        ISElement GetElement();
        ISElement GetElement(SWorldLayer worldLayer);
        bool TryGetElement(out ISElement value);
        bool TryGetElement(SWorldLayer worldLayer, out ISElement value);

        ReadOnlySpan<ISWorldSlot> GetNeighboringSlots();
        bool TryGetNeighboringSlots(out ISWorldSlot[] neighbors);

        ISWorldSlot GetWorldSlot();
        bool TryGetWorldSlot(out ISWorldSlot value);

        void SetElementTemperature(short value);
        void SetElementTemperature(SWorldLayer worldLayer, short value);
        bool TrySetElementTemperature(short value);
        bool TrySetElementTemperature(SWorldLayer worldLayer, short value);

        void SetElementFreeFalling(bool value);
        void SetElementFreeFalling(SWorldLayer worldLayer, bool value);
        bool TrySetElementFreeFalling(bool value);
        bool TrySetElementFreeFalling(SWorldLayer worldLayer, bool value);

        void SetElementColorModifier(Color value);
        void SetElementColorModifier(SWorldLayer worldLayer, Color value);
        bool TrySetElementColorModifier(Color value);
        bool TrySetElementColorModifier(SWorldLayer worldLayer, Color value);

        void NotifyChunk();
        void NotifyChunk(Point position);
        bool TryNotifyChunk();
        bool TryNotifyChunk(Point position);

        bool IsEmptyElementSlot();
    }
}
