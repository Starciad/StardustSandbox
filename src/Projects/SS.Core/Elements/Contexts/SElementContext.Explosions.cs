using Microsoft.Xna.Framework;

using StardustSandbox.Core.Explosions;

namespace StardustSandbox.Core.Elements.Contexts
{
    internal partial class SElementContext
    {
        public void InstantiateExplosion(SExplosionBuilder explosionBuilder)
        {
            InstantiateExplosion(this.Position, explosionBuilder);
        }

        public void InstantiateExplosion(Point position, SExplosionBuilder explosionBuilder)
        {
            this.world.InstantiateExplosion(position, explosionBuilder);
        }

        public bool TryInstantiateExplosion(SExplosionBuilder explosionBuilder)
        {
            return TryInstantiateExplosion(this.Position, explosionBuilder);
        }

        public bool TryInstantiateExplosion(Point position, SExplosionBuilder explosionBuilder)
        {
            return this.world.TryInstantiateExplosion(position, explosionBuilder);
        }
    }
}
