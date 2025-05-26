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

        internal SDynamite(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.OrangeRed;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_32");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism(gameInstance));
            this.enableNeighborsAction = true;
            this.defaultTemperature = 22;
            this.defaultDensity = 2400;
            this.defaultExplosionResistance = 0.5f;
        }

        protected override void OnDestroyed()
        {
            this.Context.InstantiateExplosion(explosionBuilder);
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
