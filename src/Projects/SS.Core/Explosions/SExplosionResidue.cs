namespace StardustSandbox.Core.Explosions
{
    public readonly struct SExplosionResidue(string identifier, int creationChance)
    {
        public readonly string Identifier => identifier;
        public readonly int CreationChance => creationChance;
    }
}
