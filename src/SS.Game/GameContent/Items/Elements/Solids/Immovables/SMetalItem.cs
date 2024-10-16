using StardustSandbox.Game.GameContent.Elements.Solids.Immovables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Solids.Immovables
{
    public sealed class SMetalItem : SItem
    {
        public SMetalItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_METAL";
            this.Name = "Metal";
            this.Description = string.Empty;
            this.Category = "Solids";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_13");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SMetal);
        }
    }
}
