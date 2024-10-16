﻿using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Elements.Templates.Solids.Immovables;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Immovables
{
    public sealed class SWall : SImmovableSolid
    {
        public SWall(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 013;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_14");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.EnableTemperature = false;
        }
    }
}
