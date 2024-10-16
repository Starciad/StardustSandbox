using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Common.Liquid;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    public sealed class SStoneItem : SItem
    {
        public SStoneItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_STONE";
            this.Name = "Stone";
            this.Description = string.Empty;
            this.Category = "Powders";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_4");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SStone);
        }
    }
}
