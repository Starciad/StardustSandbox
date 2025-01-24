using StardustSandbox.Core.Explosions;

namespace StardustSandbox.Core.Elements.Contexts
{
    internal partial class SElementContext
    {
        public void InstantiateExplosion(SExplosionBuilder explosionBuilder)
        {
            this.world.InstantiateExplosion(explosionBuilder);
        }

        public bool TryInstantiateExplosion(SExplosionBuilder explosionBuilder)
        {
            return this.world.TryInstantiateExplosion(explosionBuilder);
        }
    }
}
