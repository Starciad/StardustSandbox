using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    [SGameContent]
    [SElementRegister(7)]
    [SItemRegister(typeof(SSnowItem))]
    public sealed class SSnow : SMovableSolid
    {
        private sealed class SSnowItem : SItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_SNOW";
                this.Name = "Snow";
                this.Description = string.Empty;
                this.Category = "Powders";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_8");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SSnow);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.SGameInstance.AssetDatabase.GetTexture("element_8");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = -5;
        }
    }
}
