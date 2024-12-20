using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.Core.Interfaces.Elements
{
    public interface ISElementManager
    {
        void InstantiateElement<T>(SWorldLayer worldLayer, Point position) where T : ISElement;
        void InstantiateElement(SWorldLayer worldLayer, Point position, uint identifier);
        void InstantiateElement(SWorldLayer worldLayer, Point position, ISElement value);
        bool TryInstantiateElement<T>(SWorldLayer worldLayer, Point position) where T : ISElement;
        bool TryInstantiateElement(SWorldLayer worldLayer, Point position, uint identifier);
        bool TryInstantiateElement(SWorldLayer worldLayer, Point position, ISElement value);

        void UpdateElementPosition(SWorldLayer worldLayer, Point oldPosition, Point newPosition);
        bool TryUpdateElementPosition(SWorldLayer worldLayer, Point oldPosition, Point newPosition);

        void SwappingElements(SWorldLayer worldLayer, Point element1Position, Point element2Position);
        bool TrySwappingElements(SWorldLayer worldLayer, Point element1Position, Point element2Position);

        void DestroyElement(SWorldLayer worldLayer, Point position);
        bool TryDestroyElement(SWorldLayer worldLayer, Point position);

        void ReplaceElement<T>(SWorldLayer worldLayer, Point position) where T : ISElement;
        void ReplaceElement(SWorldLayer worldLayer, Point position, uint identifier);
        void ReplaceElement(SWorldLayer worldLayer, Point position, ISElement value);
        bool TryReplaceElement<T>(SWorldLayer worldLayer, Point position) where T : ISElement;
        bool TryReplaceElement(SWorldLayer worldLayer, Point position, uint identifier);
        bool TryReplaceElement(SWorldLayer worldLayer, Point position, ISElement value);

        ISElement GetElement(SWorldLayer worldLayer, Point position);
        bool TryGetElement(SWorldLayer worldLayer, Point position, out ISElement value);

        ReadOnlySpan<ISWorldSlot> GetNeighboringSlots(Point position);
        bool TryGetNeighboringSlots(Point position, out ISWorldSlot[] neighbors);

        ISWorldSlot GetElementSlot(SWorldLayer worldLayer, Point position);
        bool TryGetElementSlot(SWorldLayer worldLayer, Point position, out ISWorldSlot value);

        void SetElementTemperature(SWorldLayer worldLayer, Point position, short value);
        bool TrySetElementTemperature(SWorldLayer worldLayer, Point position, short value);

        void SetElementFreeFalling(SWorldLayer worldLayer, Point position, bool value);
        bool TrySetElementFreeFalling(SWorldLayer worldLayer, Point position, bool value);

        void SetElementColorModifier(SWorldLayer worldLayer, Point position, Color value);
        bool TrySetElementColorModifier(SWorldLayer worldLayer, Point position, Color value);

        bool IsEmptyElementSlot(Point position);
    }
}
