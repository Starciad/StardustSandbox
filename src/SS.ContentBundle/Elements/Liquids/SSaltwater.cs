using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.Elements.Liquids
{
    internal sealed class SSaltwater : SLiquid
    {
        internal SSaltwater(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(62, 182, 249, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_30");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
            this.defaultDispersionRate = 3;
            this.defaultTemperature = 25;
            this.defaultDensity = 1200;
            this.defaultExplosionResistance = 0.2f;
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            foreach (SWorldSlot neighbor in neighbors)
            {
                switch (neighbor.GetLayer(this.Context.Layer).Element)
                {
                    case SDirt:
                        this.Context.DestroyElement();
                        this.Context.ReplaceElement(neighbor.Position, this.Context.Layer, SElementConstants.MUD_IDENTIFIER);
                        break;

                    case SStone:
                        if (SRandomMath.Range(0, 150) == 0)
                        {
                            this.Context.DestroyElement();
                            this.Context.ReplaceElement(neighbor.Position, this.Context.Layer, SElementConstants.SAND_IDENTIFIER);
                        }
                        break;

                    case SFire:
                        this.Context.DestroyElement(neighbor.Position, this.Context.Layer);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue <= 21)
            {
                this.Context.ReplaceElement(SElementConstants.ICE_IDENTIFIER);
                this.Context.SetStoredElement(SElementConstants.SALTWATER_IDENTIFIER);
                return;
            }

            if (currentValue >= 110)
            {
                if (SRandomMath.Chance(50))
                {
                    this.Context.ReplaceElement(SElementConstants.STEAM_IDENTIFIER);
                }
                else
                {
                    this.Context.ReplaceElement(SElementConstants.SALT_IDENTIFIER);
                }

                return;
            }
        }
    }
}
