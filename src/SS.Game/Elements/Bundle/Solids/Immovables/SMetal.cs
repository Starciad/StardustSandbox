﻿using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Bundle.Liquids;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Immovables
{
    public sealed class SMetal : SImmovableSolid
    {
        public SMetal(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 012;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_13");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 30;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 1200)
            {
                this.Context.ReplaceElement<SLava>();
            }
        }
    }
}