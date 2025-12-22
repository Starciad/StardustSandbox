using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Explosions
{
    internal readonly struct ExplosionBuilder
    {
        internal readonly float Radius { get; init; }
        internal readonly float Power { get; init; }
        internal readonly float Heat { get; init; }

        internal readonly bool AffectsWater { get; init; }
        internal readonly bool AffectsSolids { get; init; }
        internal readonly bool AffectsGases { get; init; }

        internal readonly ElementIndex[] ExplosionResidues { get; init; }
    }
}
