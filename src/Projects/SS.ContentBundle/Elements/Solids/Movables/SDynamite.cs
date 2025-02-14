using Microsoft.Xna.Framework.Audio;

using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SDynamite : SMovableSolid
    {
        private static readonly SExplosionBuilder explosionBuilder = new()
        {
            Radius = 10,
            Power = 5f,
            Heat = 850,

            AffectsWater = true,
            AffectsSolids = true,
            AffectsGases = true,

            ExplosionResidues =
            [
                new(SElementConstants.FIRE_IDENTIFIER, 80),
                new(SElementConstants.SMOKE_IDENTIFIER, 90),
                new(SElementConstants.STONE_IDENTIFIER, 90)
            ]
        };

        private readonly SoundEffect explosionSoundEffect;

        internal SDynamite(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.OrangeRed;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_32");
            this.explosionSoundEffect = gameInstance.AssetDatabase.GetSoundEffect("sound_explosion_1");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism(gameInstance));
            this.enableNeighborsAction = true;
            this.defaultTemperature = 22;
            this.defaultDensity = 2400;
            this.defaultExplosionResistance = 0.5f;
        }

        protected override void OnDestroyed()
        {
            this.Context.InstantiateExplosion(explosionBuilder);
            SSoundEngine.Play(this.explosionSoundEffect);
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            foreach (SWorldSlot neighbor in neighbors)
            {
                SWorldSlotLayer worldSlotLayer = neighbor.GetLayer(this.Context.Layer);

                switch (worldSlotLayer.Element)
                {
                    case SFire:
                    case SLava:
                        this.Context.DestroyElement();
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 150)
            {
                this.Context.DestroyElement();
            }
        }
    }
}
