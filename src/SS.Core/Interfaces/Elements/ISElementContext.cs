using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.Core.Interfaces.Elements
{
    public interface ISElementContext : ISElementManager
    {
        ISWorldSlot Slot { get; }

        void SetPosition(SWorldLayer worldLayer, Point newPosition);
        bool TrySetPosition(SWorldLayer worldLayer, Point newPosition);

        void InstantiateElement<T>(SWorldLayer worldLayer) where T : ISElement;
        void InstantiateElement(SWorldLayer worldLayer, uint identifier);
        void InstantiateElement(SWorldLayer worldLayer, ISElement value);
        bool TryInstantiateElement<T>(SWorldLayer worldLayer) where T : ISElement;
        bool TryInstantiateElement(SWorldLayer worldLayer, uint identifier);
        bool TryInstantiateElement(SWorldLayer worldLayer, ISElement value);

        void UpdateElementPosition(SWorldLayer worldLayer, Point newPosition);
        bool TryUpdateElementPosition(SWorldLayer worldLayer, Point newPosition);

        void SwappingElements(SWorldLayer worldLayer, Point targetPosition);
        bool TrySwappingElements(SWorldLayer worldLayer, Point targetPosition);

        void DestroyElement(SWorldLayer worldLayer);
        bool TryDestroyElement(SWorldLayer worldLayer);

        void ReplaceElement<T>(SWorldLayer worldLayer) where T : ISElement;
        void ReplaceElement(SWorldLayer worldLayer, uint identifier);
        void ReplaceElement(SWorldLayer worldLayer, ISElement value);
        bool TryReplaceElement<T>(SWorldLayer worldLayer) where T : ISElement;
        bool TryReplaceElement(SWorldLayer worldLayer, uint identifier);
        bool TryReplaceElement(SWorldLayer worldLayer, ISElement value);

        ISElement GetElement(SWorldLayer worldLayer);
        bool TryGetElement(SWorldLayer worldLayer, out ISElement value);

        ReadOnlySpan<ISWorldSlot> GetNeighboringSlots();
        bool TryGetNeighboringSlots(out ISWorldSlot[] neighbors);

        ISWorldSlot GetElementSlot(SWorldLayer worldLayer);
        bool TryGetElementSlot(SWorldLayer worldLayer, out ISWorldSlot value);

        void SetElementTemperature(SWorldLayer worldLayer, short value);
        bool TrySetElementTemperature(SWorldLayer worldLayer, short value);

        void SetElementFreeFalling(SWorldLayer worldLayer, bool value);
        bool TrySetElementFreeFalling(SWorldLayer worldLayer, bool value);

        void SetElementColorModifier(SWorldLayer worldLayer, Color value);
        bool TrySetElementColorModifier(SWorldLayer worldLayer, Color value);

        void NotifyChunk();
        void NotifyChunk(Point position);
        bool TryNotifyChunk();
        bool TryNotifyChunk(Point position);

        bool IsEmptyElementSlot();
    }
}
