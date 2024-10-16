using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Immovable
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
