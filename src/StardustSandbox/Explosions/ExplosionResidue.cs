using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Explosions
{
    internal readonly struct ExplosionResidue(ElementIndex index, int creationChance)
    {
        internal readonly ElementIndex Index => index;
        internal readonly int CreationChance => creationChance;
    }
}
