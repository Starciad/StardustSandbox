using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    public sealed class SGrassItem : SItem
    {
        public SGrassItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_GRASS";
            this.Name = "Grass";
            this.Description = string.Empty;
            this.Category = "Powders";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_5");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SGrass);
        }
    }
}
