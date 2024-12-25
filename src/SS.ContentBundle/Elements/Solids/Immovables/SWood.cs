﻿using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class SWood : SImmovableSolid
    {
        internal SWood(ISGame gameInstance) : base(gameInstance)
        {
            this.identifier = (uint)SElementId.Wood;
            this.referenceColor = new(67, 34, 0, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_15");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 20;
            this.enableFlammability = true;
            this.defaultFlammabilityResistance = 35;
            this.defaultDensity = 700;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 300)
            {
                this.Context.ReplaceElement<SFire>();
            }
        }
    }
}
