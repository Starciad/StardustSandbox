using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
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
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_GLASS";
                this.Name = "Glass";
                this.Description = string.Empty;
                this.Category = "Solids";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_12");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SGlass);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.SGameInstance.AssetDatabase.GetTexture("element_12");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 25;
        }
    }
}