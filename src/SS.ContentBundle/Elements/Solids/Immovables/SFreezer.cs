﻿using StardustSandbox.ContentBundle.Elements.Utilities;
using StardustSandbox.ContentBundle.Enums.Elements.Utilities;
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
            this.enableFlammability = true;
            this.defaultTemperature = 0;
            this.defaultDensity = 1500;
            this.defaultExplosionResistance = 2.5f;
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            STemperatureUtilities.ModifyNeighborsTemperature(this.Context, neighbors, STemperatureModifierMode.Cooling);
        }
    }
}
