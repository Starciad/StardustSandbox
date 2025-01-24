using Microsoft.Xna.Framework;

using StardustSandbox.Core.Explosions;

namespace StardustSandbox.Core.Interfaces.Explosions
{
    public interface ISExplosionHandler
    {
        void InstantiateExplosion(Point position, SExplosionBuilder explosionBuilder);
        bool TryInstantiateExplosion(Point position, SExplosionBuilder explosionBuilder);
    }
}
