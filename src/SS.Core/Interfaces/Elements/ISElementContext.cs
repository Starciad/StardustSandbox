using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.World;

namespace StardustSandbox.Core.Interfaces.Elements
{
    public interface ISElementContext : ISElementManager
    {
        ISWorldSlot Slot { get; }
        ISElement Element { get; }
        Point Position { get; }

        void SetPosition(Point newPosition);
        bool TrySetPosition(Point newPosition);

        void InstantiateElement<T>() where T : ISElement;
        void InstantiateElement(uint identifier);
        void InstantiateElement(ISElement value);
        bool TryInstantiateElement<T>() where T : ISElement;
        bool TryInstantiateElement(uint identifier);
        bool TryInstantiateElement(ISElement value);

        void UpdateElementPosition(Point newPosition);
        bool TryUpdateElementPosition(Point newPosition);

        void SwappingElements(Point targetPosition);
        bool TrySwappingElements(Point targetPosition);

        void DestroyElement();
        bool TryDestroyElement();

        void ReplaceElement<T>() where T : ISElement;
        void ReplaceElement(uint identifier);
        void ReplaceElement(ISElement value);
        bool TryReplaceElement<T>() where T : ISElement;
        bool TryReplaceElement(uint identifier);
        bool TryReplaceElement(ISElement value);

        ISElement GetElement();
        bool TryGetElement(out ISElement value);

        ISWorldSlot[] GetElementNeighbors();
        bool TryGetElementNeighbors(out ISWorldSlot[] neighbors);

        ISWorldSlot GetElementSlot();
        bool TryGetElementSlot(out ISWorldSlot value);

        void SetElementTemperature(short value);
        bool TrySetElementTemperature(short value);

        void SetElementFreeFalling(bool value);
        bool TrySetElementFreeFalling(bool value);

        void SetElementColor(Color value);
        bool TryElementSetColor(Color value);

        void NotifyChunk();
        void NotifyChunk(Point position);
        bool TryNotifyChunk();
        bool TryNotifyChunk(Point position);

        bool IsEmptyElementSlot();
    }
}
