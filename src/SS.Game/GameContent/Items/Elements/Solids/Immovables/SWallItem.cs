using StardustSandbox.Game.GameContent.Elements.Solids.Immovables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Solids.Immovables
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
