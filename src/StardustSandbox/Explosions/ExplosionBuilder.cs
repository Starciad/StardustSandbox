using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Explosions
{
    internal sealed class ExplosionBuilder
    {
        internal float Radius { get; init; }
        internal float Power { get; init; }
        internal float Heat { get; init; }

        internal bool AffectsWater { get; init; }
        internal bool AffectsSolids { get; init; }
        internal bool AffectsGases { get; init; }

        internal ElementIndex[] ExplosionResidues { get; init; }
    }
}
