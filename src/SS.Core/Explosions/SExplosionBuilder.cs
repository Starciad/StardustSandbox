using System.Collections.Generic;

namespace StardustSandbox.Core.Explosions
{
    public sealed class SExplosionBuilder
    {
        public byte Radius { get; init; }
        public float Power { get; init; }
        public short Heat { get; init; }

        public bool AffectsWater { get; init; }
        public bool AffectsSolids { get; init; }
        public bool AffectsGases { get; init; }

        public IEnumerable<SExplosionResidue> ExplosionResidues { get; init; }
    }
}
