using PixelDust.Core.Worlding.World.Slots;
using PixelDust.Mathematics;
using System;

namespace PixelDust.Core.Elements.Interfaces
{
    internal interface IElementManager
    {
        void InstantiateElement<T>(Vector2Int pos) where T : PElement;
        void InstantiateElement(Vector2Int pos, uint id);
        void InstantiateElement(Vector2Int pos, PElement value);
        bool TryInstantiateElement<T>(Vector2Int pos) where T : PElement;
        bool TryInstantiateElement(Vector2Int pos, uint id);
        bool TryInstantiateElement(Vector2Int pos, PElement value);

        void UpdateElementPosition(Vector2Int oldPos, Vector2Int newPos);
        bool TryUpdateElementPosition(Vector2Int oldPos, Vector2Int newPos);

        void SwappingElements(Vector2Int element1, Vector2Int element2);
        bool TrySwappingElements(Vector2Int element1, Vector2Int element2);

        void DestroyElement(Vector2Int pos);
        bool TryDestroyElement(Vector2Int pos);

        void ReplaceElement<T>(Vector2Int pos) where T : PElement;
        void ReplaceElement(Vector2Int pos, uint id);
        void ReplaceElement(Vector2Int pos, PElement value);
        bool TryReplaceElement<T>(Vector2Int pos) where T : PElement;
        bool TryReplaceElement(Vector2Int pos, uint id);
        bool TryReplaceElement(Vector2Int pos, PElement value);

        PElement GetElement(Vector2Int pos);
        bool TryGetElement(Vector2Int pos, out PElement value);

        ReadOnlySpan<(Vector2Int, PWorldElementSlot)> GetElementNeighbors(Vector2Int pos);
        bool TryGetElementNeighbors(Vector2Int pos, out ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors);

        PWorldElementSlot GetElementSlot(Vector2Int pos);
        bool TryGetElementSlot(Vector2Int pos, out PWorldElementSlot value);

        void SetElementTemperature(Vector2Int pos, short value);
        bool TrySetElementTemperature(Vector2Int pos, short value);

        bool IsEmptyElementSlot(Vector2Int pos);
    }
}
