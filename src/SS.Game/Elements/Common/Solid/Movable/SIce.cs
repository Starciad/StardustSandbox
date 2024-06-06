using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
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
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_ICE";
                this.Name = "Ice";
                this.Description = string.Empty;
                this.Category = "Powders";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_6");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SIce);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_6");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 0;
        }
    }
}
