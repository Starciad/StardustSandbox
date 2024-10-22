using Microsoft.Xna.Framework;

using StardustSandbox.Game.Interfaces.Elements;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements.Contexts
{
    public interface ISElementContext : ISElementManager
    {
        SWorldSlot Slot { get; }
        Point Position { get; }
        SElement Element { get; }

        void SetPosition(Point newPos);
        bool TrySetPosition(Point newPos);

        void InstantiateElement<T>() where T : SElement;
        void InstantiateElement(uint id);
        void InstantiateElement(SElement value);
        bool TryInstantiateElement<T>() where T : SElement;
        bool TryInstantiateElement(uint id);
        bool TryInstantiateElement(SElement value);

        void UpdateElementPosition(Point newPos);
        bool TryUpdateElementPosition(Point newPos);

        void SwappingElements(Point targetPos);
        bool TrySwappingElements(Point targetPos);

        void DestroyElement();
        bool TryDestroyElement();

        void ReplaceElement<T>() where T : SElement;
        void ReplaceElement(uint id);
        void ReplaceElement(SElement value);
        bool TryReplaceElement<T>() where T : SElement;
        bool TryReplaceElement(uint id);
        bool TryReplaceElement(SElement value);

        SElement GetElement();
        bool TryGetElement(out SElement value);

        ReadOnlySpan<(Point, SWorldSlot)> GetElementNeighbors();
        bool TryGetElementNeighbors(out ReadOnlySpan<(Point, SWorldSlot)> neighbors);

        SWorldSlot GetElementSlot();
        bool TryGetElementSlot(out SWorldSlot value);

        void SetElementTemperature(short value);
        bool TrySetElementTemperature(short value);

        void SetElementFreeFalling(bool value);
        bool TrySetElementFreeFalling(bool value);

        bool IsEmptyElementSlot();
    }
}
