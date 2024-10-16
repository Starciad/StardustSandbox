using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    [SGameContent]
    [SElementRegister(6)]
    [SItemRegister(typeof(SSandItem))]
    public sealed class SSand : SMovableSolid
    {
        private sealed class SSandItem : SItem
        {
            public SSandItem(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance, assetDatabase)
            {
                this.Identifier = "ELEMENT_SAND";
                this.Name = "Sand";
                this.Description = string.Empty;
                this.Category = "Powders";
                this.IconTexture = assetDatabase.GetTexture("icon_element_7");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SSand);
            }
        }

        public SSand(SGame gameInstance) : base(gameInstance)
        {
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_7");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 22;
        }
    }
}
