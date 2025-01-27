using Microsoft.Xna.Framework.Audio;

using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
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
    internal sealed class SBomb : SMovableSolid
    {
        private static readonly SExplosionBuilder explosionBuilder = new()
        {
            Radius = 4,
            Power = 2.5f,
            Heat = 180,

            AffectsWater = false,
            AffectsSolids = true,
            AffectsGases = true,

            Color = SColorPalette.OrangeRed,
            CreatesLight = true,
            LightIntensity = 0.5f,

            ExplosionResidues =
            [
                new(SElementConstants.FIRE_IDENTIFIER, 65),
                new(SElementConstants.SMOKE_IDENTIFIER, 70)
            ]
        };

        private readonly SoundEffect explosionSoundEffect;

        internal SBomb(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.Charcoal;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_31");
            this.explosionSoundEffect = gameInstance.AssetDatabase.GetSoundEffect("sound_explosion_1");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism(gameInstance));
            this.enableNeighborsAction = true;
            this.defaultTemperature = 25;
            this.defaultDensity = 3500;
            this.defaultExplosionResistance = 0.3f;
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
                    case SBomb:
                    case SWall:
                    case SClone:
                    case SVoid:
                        break;

                    default:
                        this.Context.DestroyElement();
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 100)
            {
                this.Context.DestroyElement();
            }
        }
    }
}
