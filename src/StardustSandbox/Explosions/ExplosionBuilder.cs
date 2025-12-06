using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Explosions
{
    internal sealed class ExplosionBuilder
    {
        internal byte Radius { get; init; }
        internal float Power { get; init; }
        internal short Heat { get; init; }

        internal bool AffectsWater { get; init; }
        internal bool AffectsSolids { get; init; }
        internal bool AffectsGases { get; init; }

        internal ElementIndex[] ExplosionResidues { get; init; }
    }
}
