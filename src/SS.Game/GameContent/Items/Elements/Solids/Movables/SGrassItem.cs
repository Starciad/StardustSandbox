using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Solids.Movables
{
    public sealed class SGrassItem : SItem
    {
        public SGrassItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_GRASS";
            this.Name = "Grass";
            this.Description = string.Empty;
            this.Category = "Powders";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_5");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SGrass);
        }
    }
}
