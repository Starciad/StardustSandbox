using StardustSandbox.Game.GameContent.Elements.Gases;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Gases
{
    public sealed class SSmokeItem : SItem
    {
        public SSmokeItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_SMOKE";
            this.Name = "Smoke";
            this.Description = string.Empty;
            this.Category = "Gases";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_20");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SSmoke);
        }
    }
}
