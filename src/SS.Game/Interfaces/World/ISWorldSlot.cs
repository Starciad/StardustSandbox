using Microsoft.Xna.Framework;

using StardustSandbox.Game.Interfaces.Elements;
using StardustSandbox.Game.Interfaces.General;

namespace StardustSandbox.Game.Interfaces.World
{
    public interface ISWorldSlot : ISPoolableObject
    {
        ISElement Element { get; }
        bool IsEmpty { get; }
        short Temperature { get; }
        bool FreeFalling { get; }
        Color Color { get; }
    }
}
