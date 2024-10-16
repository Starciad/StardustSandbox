using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Immovable
{
    public sealed class SWallItem : SItem
    {
        public SWallItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_WALL";
            this.Name = "Wall";
            this.Description = string.Empty;
            this.Category = "Solids";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_14");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SWall);
        }
    }
}
