using Microsoft.Xna.Framework;

using StardustSandbox.Game.Interfaces.World;

using System;

namespace StardustSandbox.Game.Interfaces.Elements
{
    public interface ISElementManager
    {
        void InstantiateElement<T>(Point pos) where T : ISElement;
        void InstantiateElement(Point pos, uint id);
        void InstantiateElement(Point pos, ISElement value);
        bool TryInstantiateElement<T>(Point pos) where T : ISElement;
        bool TryInstantiateElement(Point pos, uint id);
        bool TryInstantiateElement(Point pos, ISElement value);

        void UpdateElementPosition(Point oldPos, Point newPos);
        bool TryUpdateElementPosition(Point oldPos, Point newPos);

        void SwappingElements(Point element1Pos, Point element2Pos);
        bool TrySwappingElements(Point element1Pos, Point element2Pos);

        void DestroyElement(Point pos);
        bool TryDestroyElement(Point pos);

        void ReplaceElement<T>(Point pos) where T : ISElement;
        void ReplaceElement(Point pos, uint id);
        void ReplaceElement(Point pos, ISElement value);
        bool TryReplaceElement<T>(Point pos) where T : ISElement;
        bool TryReplaceElement(Point pos, uint id);
        bool TryReplaceElement(Point pos, ISElement value);

        ISElement GetElement(Point pos);
        bool TryGetElement(Point pos, out ISElement value);

        ReadOnlySpan<(Point, ISWorldSlot)> GetElementNeighbors(Point pos);
        bool TryGetElementNeighbors(Point pos, out ReadOnlySpan<(Point, ISWorldSlot)> neighbors);

        ISWorldSlot GetElementSlot(Point pos);
        bool TryGetElementSlot(Point pos, out ISWorldSlot value);

        void SetElementTemperature(Point pos, short value);
        bool TrySetElementTemperature(Point pos, short value);

        void SetElementFreeFalling(Point pos, bool value);
        bool TrySetElementFreeFalling(Point pos, bool value);

        void SetElementColor(Point pos, Color value);
        bool TrySetElementColor(Point pos, Color value);

        bool IsEmptyElementSlot(Point pos);
    }
}
