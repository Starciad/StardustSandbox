using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Elements.Common.Liquid;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    [SGameContent]
    [SElementRegister(3)]
    [SItemRegister(typeof(SStoneItem))]
    public sealed class SStone : SMovableSolid
    {
        private sealed class SStoneItem : SItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_STONE";
                this.Name = "Stone";
                this.Description = string.Empty;
                this.Category = "Powders";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_4");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SStone);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.SGameInstance.AssetDatabase.GetTexture("element_4");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 500)
            {
                this.Context.ReplaceElement<SLava>();
                this.Context.SetElementTemperature(600);
            }
        }
    }
}
