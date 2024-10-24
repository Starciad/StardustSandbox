﻿using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.GameContent.Elements.Liquids;
using StardustSandbox.Game.GameContent.Elements.Rendering;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Movables
{
    public sealed class SIce : SMovableSolid
    {
        public SIce(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 005;
            this.Texture = this.SGameInstance.AssetDatabase.GetTexture("element_6");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = -25;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 0)
            {
                this.Context.ReplaceElement<SWater>();
            }
        }
    }
}
