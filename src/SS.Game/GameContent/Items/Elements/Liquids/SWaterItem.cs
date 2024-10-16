using StardustSandbox.Game.GameContent.Elements.Liquids;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Liquids
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