using Microsoft.Xna.Framework.Audio;

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.World.Slots;
using StardustSandbox.GameContent.Elements.Energies;
using StardustSandbox.GameContent.Elements.Liquids;

using System.Collections.Generic;

namespace StardustSandbox.GameContent.Elements.Solids.Movables
{
    internal sealed class STnt : SMovableSolid
    {
        private static readonly SExplosionBuilder explosionBuilder = new()
        {
            Radius = 6,
            Power = 5f,
            Heat = 450,

            AffectsWater = false,
            AffectsSolids = true,
            AffectsGases = true,

            ExplosionResidues =
            [
                new(SElementConstants.FIRE_IDENTIFIER, 65),
                new(SElementConstants.SMOKE_IDENTIFIER, 65)
            ]
        };
        private readonly SoundEffect explosionSoundEffect;

        internal STnt(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.DarkRed;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_33");
            this.explosionSoundEffect = gameInstance.AssetDatabase.GetSoundEffect("sound_explosion_1");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism(gameInstance));
            this.enableNeighborsAction = true;
            this.defaultTemperature = 22;
            this.defaultDensity = 2800;
            this.defaultExplosionResistance = 0.35f;
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
            if (currentValue > 120)
            {
                this.Context.DestroyElement();
            }
        }
    }
}
