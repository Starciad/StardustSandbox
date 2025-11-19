using Microsoft.Xna.Framework;

using StardustSandbox.Interfaces.Collections;

using System.Collections.Generic;

namespace StardustSandbox.ExplosionSystem
{
    internal class Explosion : IPoolableObject
    {
        internal Point Position { get; private set; }

        internal byte Radius { get; private set; }
        internal float Power { get; private set; }
        internal short Heat { get; private set; }

        internal bool AffectsWater { get; private set; }
        internal bool AffectsSolids { get; private set; }
        internal bool AffectsGases { get; private set; }

        internal IEnumerable<ExplosionResidue> ExplosionResidues { get; private set; }

        internal void Build(Point position, ExplosionBuilder builder)
        {
            this.Position = position;

            this.Radius = builder.Radius;
            this.Power = builder.Power;
            this.Heat = builder.Heat;

            this.AffectsWater = builder.AffectsWater;
            this.AffectsSolids = builder.AffectsSolids;
            this.AffectsGases = builder.AffectsGases;

            this.ExplosionResidues = builder.ExplosionResidues;
        }

        public void Reset()
        {
            this.Position = Point.Zero;

            this.Radius = 0;
            this.Power = 0f;
            this.Heat = 0;

            this.AffectsWater = false;
            this.AffectsSolids = false;
            this.AffectsGases = false;

            this.ExplosionResidues = null;
        }
    }
}
