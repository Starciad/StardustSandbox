using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    [SGameContent]
    [SElementRegister(5)]
    [SItemRegister(typeof(SIceItem))]
    public sealed class SIce : SMovableSolid
    {
        private sealed class SIceItem : SItem
        {
            public SIceItem(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance, assetDatabase)
            {
                this.Identifier = "ELEMENT_ICE";
                this.Name = "Ice";
                this.Description = string.Empty;
                this.Category = "Powders";
                this.IconTexture = assetDatabase.GetTexture("icon_element_6");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SIce);
            }
        }

        public SIce(SGame gameInstance) : base(gameInstance)
        {
            this.Texture = this.SGameInstance.AssetDatabase.GetTexture("element_6");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 0;
        }
    }
}
