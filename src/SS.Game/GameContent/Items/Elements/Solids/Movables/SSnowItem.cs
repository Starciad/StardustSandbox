using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Solids.Movables
{
    public sealed class SSnowItem : SItem
    {
        public SSnowItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_SNOW";
            this.Name = "Snow";
            this.Description = string.Empty;
            this.Category = "Powders";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_8");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SSnow);
        }
    }
}
