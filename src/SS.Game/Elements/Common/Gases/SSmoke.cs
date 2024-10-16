using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Gases
{
    [SGameContent]
    [SElementRegister(19)]
    [SItemRegister(typeof(SSmokeItem))]
    public class SSmoke : SGas
    {
        private sealed class SSmokeItem : SItem
        {
            public SSmokeItem(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance, assetDatabase)
            {
                this.Identifier = "ELEMENT_SMOKE";
                this.Name = "Smoke";
                this.Description = string.Empty;
                this.Category = "Gases";
                this.IconTexture = assetDatabase.GetTexture("icon_element_20");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SSmoke);
            }
        }

        public SSmoke(SGame gameInstance) : base(gameInstance)
        {
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_20");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 100;
        }
    }
}
