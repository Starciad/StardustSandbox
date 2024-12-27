using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.World.Data;

using System.Collections.Generic;

namespace StardustSandbox.Core.Interfaces.Elements
{
    public interface ISElementManager
    {
        void InstantiateElement(Point position, SWorldLayer worldLayer, string identifier);
        void InstantiateElement(Point position, SWorldLayer worldLayer, ISElement value);
        bool TryInstantiateElement(Point position, SWorldLayer worldLayer, string identifier);
        bool TryInstantiateElement(Point position, SWorldLayer worldLayer, ISElement value);

        void UpdateElementPosition(Point oldPosition, Point newPosition, SWorldLayer worldLayer);
        bool TryUpdateElementPosition(Point oldPosition, Point newPosition, SWorldLayer worldLayer);

        void SwappingElements(Point element1Position, Point element2Position, SWorldLayer worldLayer);
        bool TrySwappingElements(Point element1Position, Point element2Position, SWorldLayer worldLayer);

        void DestroyElement(Point position, SWorldLayer worldLayer);
        bool TryDestroyElement(Point position, SWorldLayer worldLayer);

        void ReplaceElement(Point position, SWorldLayer worldLayer, string identifier);
        void ReplaceElement(Point position, SWorldLayer worldLayer, ISElement value);
        bool TryReplaceElement(Point position, SWorldLayer worldLayer, string identifier);
        bool TryReplaceElement(Point position, SWorldLayer worldLayer, ISElement value);

        ISElement GetElement(Point position, SWorldLayer worldLayer);
        bool TryGetElement(Point position, SWorldLayer worldLayer, out ISElement value);

        IEnumerable<SWorldSlot> GetNeighboringSlots(Point position);

        SWorldSlot GetWorldSlot(Point position);
        bool TryGetWorldSlot(Point position, out SWorldSlot value);

        void SetElementTemperature(Point position, SWorldLayer worldLayer, short value);
        bool TrySetElementTemperature(Point position, SWorldLayer worldLayer, short value);

        void SetElementFreeFalling(Point position, SWorldLayer worldLayer, bool value);
        bool TrySetElementFreeFalling(Point position, SWorldLayer worldLayer, bool value);

        void SetElementColorModifier(Point position, SWorldLayer worldLayer, Color value);
        bool TrySetElementColorModifier(Point position, SWorldLayer worldLayer, Color value);

        bool IsEmptyWorldSlot(Point position);
        bool IsEmptyWorldSlotLayer(Point position, SWorldLayer worldLayer);
    }
}
