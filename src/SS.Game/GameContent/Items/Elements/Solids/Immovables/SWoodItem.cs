using StardustSandbox.Game.GameContent.Elements.Solids.Immovables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Solids.Immovables
{
    public sealed class SWoodItem : SItem
    {
        public SWoodItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_WOOD";
            this.Name = "Wood";
            this.Description = string.Empty;
            this.Category = "Solids";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_15");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SWood);
        }
    }
}
