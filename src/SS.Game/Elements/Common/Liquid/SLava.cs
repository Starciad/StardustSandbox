using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Elements.Common.Solid.Movable;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Liquid
{
    [SGameContent]
    [SElementRegister(9)]
    [SItemRegister(typeof(SLavaItem))]
    public class SLava : SLiquid
    {
        private sealed class SLavaItem : SItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_LAVA";
                this.Name = "Lava";
                this.Description = string.Empty;
                this.Category = "Liquids";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_10");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SLava);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.SGameInstance.AssetDatabase.GetTexture("element_10");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 1000;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue < 500)
            {
                this.Context.ReplaceElement<SStone>();
                this.Context.SetElementTemperature(550);
            }
        }
    }
}