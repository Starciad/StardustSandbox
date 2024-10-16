using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Gases
{
    [SGameContent]
    [SElementRegister(18)]
    [SItemRegister(typeof(SSteamItem))]
    public class SSteam : SGas
    {
        private sealed class SSteamItem : SItem
        {
            public SSteamItem(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance, assetDatabase)
            {
                this.Identifier = "ELEMENT_STEAM";
                this.Name = "Steam";
                this.Description = string.Empty;
                this.Category = "Gases";
                this.IconTexture = assetDatabase.GetTexture("icon_element_19");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SSteam);
            }
        }

        public SSteam(SGame gameInstance) : base(gameInstance)
        {
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_19");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 100;
        }
    }
}
