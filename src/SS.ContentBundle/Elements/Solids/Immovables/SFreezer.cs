﻿using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class SFreezer : SImmovableSolid
    {
        internal SFreezer(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.NavyBlue;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_38");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism(gameInstance));
            this.enableNeighborsAction = true;
            this.defaultTemperature = 0;
            this.defaultDensity = 1500;
            this.defaultExplosionResistance = 2.5f;
        }

        private void Cool(Point position, SWorldSlotLayer worldSlotLayer)
        {
            if (!worldSlotLayer.Element.EnableTemperature)
            {
                return;
            }

            this.Context.SetElementTemperature(position, this.Context.Layer, (short)(worldSlotLayer.Temperature - 10));
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            foreach (SWorldSlot neighbor in neighbors)
            {
                SWorldSlotLayer worldSlotLayer = neighbor.GetLayer(this.Context.Layer);

                switch (worldSlotLayer.Element)
                {
                    case SHeater:
                    case SFreezer:
                        continue;

                    default:
                        break;
                }

                Cool(neighbor.Position, worldSlotLayer);
            }
        }
    }
}
