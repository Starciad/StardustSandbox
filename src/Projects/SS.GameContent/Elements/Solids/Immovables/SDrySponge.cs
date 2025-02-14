using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Mathematics.Geometry;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class SDrySponge : SImmovableSolid
    {
        internal SDrySponge(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.Amber;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_34");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableFlammability = true;
            this.enableNeighborsAction = true;
            this.defaultTemperature = 25;
            this.defaultFlammabilityResistance = 10;
            this.defaultDensity = 550;
            this.defaultExplosionResistance = 0.5f;
        }

        private void AbsorbWaterAround()
        {
            foreach (Point position in SShapePointGenerator.GenerateSquarePoints(this.Context.Slot.Position, 1))
            {
                if (!this.Context.TryGetWorldSlot(position, out SWorldSlot worldSlot))
                {
                    continue;
                }

                SWorldSlotLayer worldSlotLayer = worldSlot.GetLayer(this.Context.Layer);

                switch (worldSlotLayer.Element)
                {
                    case SWater:
                    case SSaltwater:
                        this.Context.DestroyElement(position, this.Context.Layer);
                        break;

                    default:
                        break;
                }
            }

            this.Context.ReplaceElement(SElementConstants.WET_SPONGE_IDENTIFIER);
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            foreach (SWorldSlot neighbor in neighbors)
            {
                SWorldSlotLayer worldSlotLayer = neighbor.GetLayer(this.Context.Layer);

                switch (worldSlotLayer.Element)
                {
                    case SWater:
                    case SSaltwater:
                        AbsorbWaterAround();
                        return;

                    default:
                        continue;
                }
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 180)
            {
                if (SRandomMath.Chance(70))
                {
                    this.Context.ReplaceElement(SElementConstants.FIRE_IDENTIFIER);
                }
                else
                {
                    this.Context.ReplaceElement(SElementConstants.ASH_IDENTIFIER);
                }
            }
        }
    }
}
