using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.Interfaces.World
{
    public interface ISWorldSlot : ISPoolableObject
    {
        bool IsEmpty { get; }
        Point Position { get; }
        ISWorldSlotLayer ForegroundLayer { get; }
        ISWorldSlotLayer BackgroundLayer { get; }

        ISWorldSlotLayer GetLayer(SWorldLayer worldLayer);
    }

    public interface ISWorldSlotLayer
    {
        ISElement Element { get; }
        bool IsEmpty { get; }
        short Temperature { get; }
        bool FreeFalling { get; }
        Color ColorModifier { get; }
        SUpdateCycleFlag UpdateCycleFlag { get; }
        SUpdateCycleFlag StepCycleFlag { get; }
    }
}
