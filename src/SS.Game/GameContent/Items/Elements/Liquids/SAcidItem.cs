using StardustSandbox.Game.GameContent.Elements.Liquids;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Liquids
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