using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Interfaces.Collections;

using System.Collections.Generic;

namespace StardustSandbox.Core.Explosions
{
    internal class SExplosion : ISPoolableObject
    {
        internal Point Position { get; private set; }

        public byte Radius { get; private set; }
        public float Power { get; private set; }
        public short Heat { get; private set; }

        public bool AffectsWater { get; private set; }
        public bool AffectsSolids { get; private set; }
        public bool AffectsGases { get; private set; }

        public Color Color { get; private set; }
        public bool CreatesLight { get; private set; }
        public float LightIntensity { get; private set; }

        public IEnumerable<SExplosionResidue> ExplosionResidues { get; private set; }

        public void Build(Point position, SExplosionBuilder builder)
        {
            this.Position = position;

            this.Radius = builder.Radius;
            this.Power = builder.Power;
            this.Heat = builder.Heat;

            this.AffectsWater = builder.AffectsWater;
            this.AffectsSolids = builder.AffectsSolids;
            this.AffectsGases = builder.AffectsGases;

            this.Color = builder.Color;
            this.CreatesLight = builder.CreatesLight;
            this.LightIntensity = builder.LightIntensity;

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

            this.Color = SColorPalette.White;
            this.CreatesLight = false;
            this.LightIntensity = 0f;

            this.ExplosionResidues = null;
        }
    }
}
