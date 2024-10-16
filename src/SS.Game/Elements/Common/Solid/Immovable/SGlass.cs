using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Immovable
{
    [SGameContent]
    [SElementRegister(11)]
    [SItemRegister(typeof(SGlassItem))]
    public sealed class SGlass : SImmovableSolid
    {
        private sealed class SGlassItem : SItem
        {
            public SGlassItem(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance, assetDatabase)
            {
                this.Identifier = "ELEMENT_GLASS";
                this.Name = "Glass";
                this.Description = string.Empty;
                this.Category = "Solids";
                this.IconTexture = assetDatabase.GetTexture("icon_element_12");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SGlass);
            }
        }

        public SGlass(SGame gameInstance) : base(gameInstance)
        {
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_12");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 25;
        }
    }
}