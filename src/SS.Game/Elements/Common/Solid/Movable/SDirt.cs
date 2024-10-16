using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    [SGameContent]
    [SElementRegister(0)]
    [SItemRegister(typeof(SDirtItem))]
    public sealed class SDirt : SMovableSolid
    {
        private sealed class SDirtItem : SItem
        {
            public SDirtItem(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance, assetDatabase)
            {
                this.Identifier = "ELEMENT_DIRT";
                this.Name = "Dirt";
                this.Description = string.Empty;
                this.Category = "Powders";
                this.IconTexture = assetDatabase.GetTexture("icon_element_1");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SDirt);
            }
        }

        public SDirt(SGame gameInstance) : base(gameInstance)
        {
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_1");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
            this.EnableNeighborsAction = true;
        }
    }
}
