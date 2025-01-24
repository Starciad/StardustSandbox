using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.Collections;

namespace StardustSandbox.Core.Explosions
{
    internal class SExplosion : ISPoolableObject
    {
        internal Point Position { get; set; }
        internal float Radius { get; set; }
        internal float Power { get; set; }

        public void Build(SExplosionBuilder builder)
        {
            this.Position = builder.Position;
            this.Radius = builder.Radius;
            this.Power = builder.Power;
        }

        public void Reset()
        {
            this.Position = Point.Zero;
            this.Radius = 0f;
            this.Power = 0f;
        }
    }
}
