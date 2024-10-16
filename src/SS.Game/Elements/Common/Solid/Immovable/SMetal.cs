using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
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
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_METAL";
                this.Name = "Metal";
                this.Description = string.Empty;
                this.Category = "Solids";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_13");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SMetal);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.SGameInstance.AssetDatabase.GetTexture("element_13");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 30;
        }
    }
}
