using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Common.Solid.Movable;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Liquid
{
    public sealed class SLavaItem : SItem
    {
        public SLavaItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_LAVA";
            this.Name = "Lava";
            this.Description = string.Empty;
            this.Category = "Liquids";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_10");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SLava);
        }
    }
}