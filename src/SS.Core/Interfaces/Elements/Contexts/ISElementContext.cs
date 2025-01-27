using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.Interfaces.Explosions;
using StardustSandbox.Core.Interfaces.Lighting;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.Core.Interfaces.Elements.Contexts
{
    public interface ISElementContext : ISElementHandler, ISExplosionHandler, ISWorldChunkingHandler, ISLightingHandler
    {
        SWorldSlot Slot { get; }
        SWorldSlotLayer SlotLayer { get; }
        SWorldLayer Layer { get; }

        void SetPosition(Point newPosition);
        void SetPosition(Point newPosition, SWorldLayer worldLayer);
        bool TrySetPosition(Point newPosition);
        bool TrySetPosition(Point newPosition, SWorldLayer worldLayer);

        void InstantiateElement(string identifier);
        void InstantiateElement(ISElement value);
        void InstantiateElement(SWorldLayer worldLayer, string identifier);
        void InstantiateElement(SWorldLayer worldLayer, ISElement value);
        bool TryInstantiateElement(string identifier);
        bool TryInstantiateElement(ISElement value);
        bool TryInstantiateElement(SWorldLayer worldLayer, string identifier);
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

        void RemoveElement();
        void RemoveElement(Point position);
        bool TryRemoveElement();
        bool TryRemoveElement(Point position);

        void ReplaceElement(string identifier);
        void ReplaceElement(SWorldLayer worldLayer, string identifier);
        void ReplaceElement(ISElement value);
        void ReplaceElement(SWorldLayer worldLayer, ISElement value);
        bool TryReplaceElement(string identifier);
        bool TryReplaceElement(ISElement value);
        bool TryReplaceElement(SWorldLayer worldLayer, string identifier);
        bool TryReplaceElement(SWorldLayer worldLayer, ISElement value);

        ISElement GetElement();
        ISElement GetElement(SWorldLayer worldLayer);
        bool TryGetElement(out ISElement value);
        bool TryGetElement(SWorldLayer worldLayer, out ISElement value);

        IEnumerable<SWorldSlot> GetNeighboringSlots();

        SWorldSlot GetWorldSlot();
        bool TryGetWorldSlot(out SWorldSlot value);

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

        void SetStoredElement(SWorldLayer worldLayer, string identifier);
        void SetStoredElement(SWorldLayer worldLayer, ISElement element);
        void SetStoredElement(string identifier);
        void SetStoredElement(ISElement element);
        bool TrySetStoredElement(SWorldLayer worldLayer, string identifier);
        bool TrySetStoredElement(SWorldLayer worldLayer, ISElement element);
        bool TrySetStoredElement(string identifier);
        bool TrySetStoredElement(ISElement element);

        void SetLightIntensity(byte value);
        void SetLightIntensity(SWorldLayer worldLayer, byte value);
        bool TrySetLightIntensity(byte value);
        bool TrySetLightIntensity(SWorldLayer worldLayer, byte value);

        void NotifyChunk();
        bool TryNotifyChunk();

        bool IsEmptyWorldSlot();
        bool IsEmptyWorldSlotLayer();
        bool IsEmptyWorldSlotLayer(SWorldLayer worldLayer);

        void InstantiateExplosion(SExplosionBuilder explosionBuilder);
        bool TryInstantiateExplosion(SExplosionBuilder explosionBuilder);

        void InstantiateLightingSource(byte intensity, Color color);
        void InstantiateLightingSource(SWorldLayer worldLayer, byte intensity, Color color);
    }
}
