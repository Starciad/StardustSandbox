using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Immovable
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