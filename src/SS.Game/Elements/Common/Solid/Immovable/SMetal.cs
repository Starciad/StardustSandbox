using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Immovable
{
    [SGameContent]
    [SElementRegister(12)]
    [SItemRegister(typeof(SMetalItem))]
    public sealed class SMetal : SImmovableSolid
    {
        private sealed class SMetalItem : SItem
        {
            public SMetalItem(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance, assetDatabase)
            {
                this.Identifier = "ELEMENT_METAL";
                this.Name = "Metal";
                this.Description = string.Empty;
                this.Category = "Solids";
                this.IconTexture = assetDatabase.GetTexture("icon_element_13");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SMetal);
            }
        }

        public SMetal(SGame gameInstance) : base(gameInstance)
        {
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_13");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 30;
        }
    }
}
