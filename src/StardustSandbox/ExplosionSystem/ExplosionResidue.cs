using StardustSandbox.Enums.Indexers;

namespace StardustSandbox.ExplosionSystem
{
    internal readonly struct ExplosionResidue(ElementIndex index, int creationChance)
    {
        internal readonly ElementIndex Index => index;
        internal readonly int CreationChance => creationChance;
    }
}
