using StardustSandbox.Game.GameContent.Elements.Solids.Immovables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Solids.Immovables
{
    public sealed class SGlassItem : SItem
    {
        public SGlassItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_GLASS";
            this.Name = "Glass";
            this.Description = string.Empty;
            this.Category = "Solids";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_12");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SGlass);
        }
    }
}