using StardustSandbox.Game.GameContent.Elements.Gases;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Gases
{
    public sealed class SSteamItem : SItem
    {
        public SSteamItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_STEAM";
            this.Name = "Steam";
            this.Description = string.Empty;
            this.Category = "Gases";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_19");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SSteam);
        }
    }
}
