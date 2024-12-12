using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.Interfaces.World
{
    public interface ISWorldSlot : ISPoolableObject
    {
        ISElement Element { get; }
        Point Position { get; }
        bool IsEmpty { get; }
        short Temperature { get; }
        bool FreeFalling { get; }
        Color Color { get; }
    }
}
