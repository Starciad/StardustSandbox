using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.Core.Interfaces.Elements
{
    public interface ISElementManager
    {
        void InstantiateElement<T>(Point position) where T : ISElement;
        void InstantiateElement(Point position, uint identifier);
        void InstantiateElement(Point position, ISElement value);
        bool TryInstantiateElement<T>(Point position) where T : ISElement;
        bool TryInstantiateElement(Point position, uint identifier);
        bool TryInstantiateElement(Point position, ISElement value);

        void UpdateElementPosition(Point oldPosition, Point newPosition);
        bool TryUpdateElementPosition(Point oldPosition, Point newPosition);

        void SwappingElements(Point element1Position, Point element2Position);
        bool TrySwappingElements(Point element1Position, Point element2Position);

        void DestroyElement(Point position);
        bool TryDestroyElement(Point position);

        void ReplaceElement<T>(Point position) where T : ISElement;
        void ReplaceElement(Point position, uint identifier);
        void ReplaceElement(Point position, ISElement value);
        bool TryReplaceElement<T>(Point position) where T : ISElement;
        bool TryReplaceElement(Point position, uint identifier);
        bool TryReplaceElement(Point position, ISElement value);

        ISElement GetElement(Point position);
        bool TryGetElement(Point position, out ISElement value);

        ReadOnlySpan<ISWorldSlot> GetElementNeighbors(Point position);
        bool TryGetElementNeighbors(Point position, out ISWorldSlot[] neighbors);

        ISWorldSlot GetElementSlot(Point position);
        bool TryGetElementSlot(Point position, out ISWorldSlot value);

        void SetElementTemperature(Point position, short value);
        bool TrySetElementTemperature(Point position, short value);

        void SetElementFreeFalling(Point position, bool value);
        bool TrySetElementFreeFalling(Point position, bool value);

        void SetElementColor(Point position, Color value);
        bool TrySetElementColor(Point position, Color value);

        bool IsEmptyElementSlot(Point position);
    }
}
