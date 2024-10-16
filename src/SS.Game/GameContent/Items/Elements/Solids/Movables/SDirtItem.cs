using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Solids.Movables
{
    public sealed class SDirtItem : SItem
    {
        public SDirtItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_DIRT";
            this.Name = "Dirt";
            this.Description = string.Empty;
            this.Category = "Powders";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_1");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SDirt);
        }
    }
}
