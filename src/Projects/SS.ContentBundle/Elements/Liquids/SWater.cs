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
    internal sealed class SWater : SLiquid
    {
        internal SWater(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(8, 120, 284, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_3");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
            this.defaultDispersionRate = 3;
            this.defaultTemperature = 25;
            this.defaultDensity = 1000;
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
            if (currentValue <= 0)
            {
                this.Context.ReplaceElement(SElementConstants.ICE_IDENTIFIER);
                return;
            }

            if (currentValue >= 100)
            {
                this.Context.ReplaceElement(SElementConstants.STEAM_IDENTIFIER);
                return;
            }
        }
    }
}