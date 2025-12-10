using System;

namespace StardustSandbox.Enums.Elements
{
    [Flags]
    internal enum ElementCharacteristics : short
    {
        None = 0,
        AffectsNeighbors = 1 << 1,
        HasTemperature = 1 << 2,
        IsFlammable = 1 << 3,
        IsExplosive = 1 << 4,
        IsExplosionImmune = 1 << 5,
        IsCorruptible = 1 << 6,
        IsCorruption = 1 << 7,
        IsPushable = 1 << 8
    }
}
