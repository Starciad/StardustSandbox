using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Solids.Movables
{
    public sealed class SSandItem : SItem
    {
        public SSandItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_SAND";
            this.Name = "Sand";
            this.Description = string.Empty;
            this.Category = "Powders";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_7");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SSand);
        }
    }
}
