using StardustSandbox.Game.GameContent.Elements.Liquids;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Liquids
{
    public sealed class SLCorruptionItem : SItem
    {
        public SLCorruptionItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_CORRUPTION_LIQUID";
            this.Name = "Corruption (Liquid)";
            this.Description = string.Empty;
            this.Category = "Liquids";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_17");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SLCorruption);
        }
    }
}
