using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Interfaces.Elements
{
    public interface ISElementManager
    {
        void InstantiateElement<T>(Point pos) where T : SElement;
        void InstantiateElement(Point pos, uint id);
        void InstantiateElement(Point pos, SElement value);
        bool TryInstantiateElement<T>(Point pos) where T : SElement;
        bool TryInstantiateElement(Point pos, uint id);
        bool TryInstantiateElement(Point pos, SElement value);

        void UpdateElementPosition(Point oldPos, Point newPos);
        bool TryUpdateElementPosition(Point oldPos, Point newPos);

        void SwappingElements(Point element1Pos, Point element2Pos);
        bool TrySwappingElements(Point element1Pos, Point element2Pos);

        void DestroyElement(Point pos);
        bool TryDestroyElement(Point pos);

        void ReplaceElement<T>(Point pos) where T : SElement;
        void ReplaceElement(Point pos, uint id);
        void ReplaceElement(Point pos, SElement value);
        bool TryReplaceElement<T>(Point pos) where T : SElement;
        bool TryReplaceElement(Point pos, uint id);
        bool TryReplaceElement(Point pos, SElement value);

        SElement GetElement(Point pos);
        bool TryGetElement(Point pos, out SElement value);

        ReadOnlySpan<(Point, SWorldSlot)> GetElementNeighbors(Point pos);
        bool TryGetElementNeighbors(Point pos, out ReadOnlySpan<(Point, SWorldSlot)> neighbors);

        SWorldSlot GetElementSlot(Point pos);
        bool TryGetElementSlot(Point pos, out SWorldSlot value);

        void SetElementTemperature(Point pos, short value);
        bool TrySetElementTemperature(Point pos, short value);

        void SetElementFreeFalling(Point pos, bool value);
        bool TrySetElementFreeFalling(Point pos, bool value);

        bool IsEmptyElementSlot(Point pos);
    }
}
