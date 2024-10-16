using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Immovable
{
    [SGameContent]
    [SElementRegister(14)]
    [SItemRegister(typeof(SWoodItem))]
    public sealed class SWood : SImmovableSolid
    {
        private sealed class SWoodItem : SItem
        {
            public SWoodItem(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance, assetDatabase)
            {
                this.Identifier = "ELEMENT_WOOD";
                this.Name = "Wood";
                this.Description = string.Empty;
                this.Category = "Solids";
                this.IconTexture = assetDatabase.GetTexture("icon_element_15");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SWood);
            }
        }

        public SWood(SGame gameInstance) : base(gameInstance)
        {
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_15");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
        }
    }
}
