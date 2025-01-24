using StardustSandbox.Core.Explosions;

namespace StardustSandbox.Core.Interfaces.Explosions
{
    public interface ISExplosionHandler
    {
        void InstantiateExplosion(SExplosionBuilder explosionBuilder);
        bool TryInstantiateExplosion(SExplosionBuilder explosionBuilder);
    }
}
