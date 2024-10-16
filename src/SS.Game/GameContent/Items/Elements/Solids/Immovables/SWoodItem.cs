using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Immovable
{
    public sealed class SWoodItem : SItem
    {
        public SWoodItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_WOOD";
            this.Name = "Wood";
            this.Description = string.Empty;
            this.Category = "Solids";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_15");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SWood);
        }
    }
}
