using Microsoft.Xna.Framework;

using PixelDust.Game.World.Data;

using System;

namespace PixelDust.Game.Elements.Interfaces
{
    public interface IElementManager
    {
        void InstantiateElement<T>(Point pos) where T : PElement;
        void InstantiateElement(Point pos, uint id);
        void InstantiateElement(Point pos, PElement value);
        bool TryInstantiateElement<T>(Point pos) where T : PElement;
        bool TryInstantiateElement(Point pos, uint id);
        bool TryInstantiateElement(Point pos, PElement value);

        void UpdateElementPosition(Point oldPos, Point newPos);
        bool TryUpdateElementPosition(Point oldPos, Point newPos);

        void SwappingElements(Point element1, Point element2);
        bool TrySwappingElements(Point element1, Point element2);

        void DestroyElement(Point pos);
        bool TryDestroyElement(Point pos);

        void ReplaceElement<T>(Point pos) where T : PElement;
        void ReplaceElement(Point pos, uint id);
        void ReplaceElement(Point pos, PElement value);
        bool TryReplaceElement<T>(Point pos) where T : PElement;
        bool TryReplaceElement(Point pos, uint id);
        bool TryReplaceElement(Point pos, PElement value);

        PElement GetElement(Point pos);
        bool TryGetElement(Point pos, out PElement value);

        ReadOnlySpan<(Point, PWorldSlot)> GetElementNeighbors(Point pos);
        bool TryGetElementNeighbors(Point pos, out ReadOnlySpan<(Point, PWorldSlot)> neighbors);

        PWorldSlot GetElementSlot(Point pos);
        bool TryGetElementSlot(Point pos, out PWorldSlot value);

        void SetElementTemperature(Point pos, short value);
        bool TrySetElementTemperature(Point pos, short value);

        bool IsEmptyElementSlot(Point pos);
    }
}
