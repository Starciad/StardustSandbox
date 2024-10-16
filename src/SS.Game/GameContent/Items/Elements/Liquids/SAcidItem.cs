using Microsoft.Xna.Framework;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Common.Solid.Immovable;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements.Common.Liquid
{
    public sealed class SAcidItem : SItem
    {
        public SAcidItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_ACID";
            this.Name = "Acid";
            this.Description = string.Empty;
            this.Category = "Liquids";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_11");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SAcid);
        }
    }
}