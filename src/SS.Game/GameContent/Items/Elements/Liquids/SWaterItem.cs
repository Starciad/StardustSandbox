using Microsoft.Xna.Framework;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Common.Gases;
using StardustSandbox.Game.Elements.Common.Solid.Movable;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.General;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements.Common.Liquid
{
    public sealed class SWaterItem : SItem
    {
        public SWaterItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_WATER";
            this.Name = "Water";
            this.Description = string.Empty;
            this.Category = "Liquids";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_3");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SWater);
        }
    }
}