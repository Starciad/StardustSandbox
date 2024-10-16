using StardustSandbox.Game.GameContent.Elements.Liquids;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Liquids
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