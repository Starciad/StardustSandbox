using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Solids.Movables
{
    public sealed class SIceItem : SItem
    {
        public SIceItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_ICE";
            this.Name = "Ice";
            this.Description = string.Empty;
            this.Category = "Powders";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_6");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SIce);
        }
    }
}
