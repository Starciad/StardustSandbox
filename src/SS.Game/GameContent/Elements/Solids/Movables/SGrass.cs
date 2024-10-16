﻿using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Elements.Templates.Solids.Movables;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Movables
{
    public sealed class SGrass : SMovableSolid
    {
        public SGrass(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 004;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_5");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 22;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 200)
            {
                this.Context.DestroyElement();
            }
        }
    }
}
