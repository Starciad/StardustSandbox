using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Solids.Movables
{
    public sealed class SStoneItem : SItem
    {
        public SStoneItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_STONE";
            this.Name = "Stone";
            this.Description = string.Empty;
            this.Category = "Powders";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_4");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SStone);
        }
    }
}
