using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    [SGameContent]
    [SElementRegister(4)]
    [SItemRegister(typeof(SGrassItem))]
    public sealed class SGrass : SMovableSolid
    {
        private sealed class SGrassItem : SItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_GRASS";
                this.Name = "Grass";
                this.Description = string.Empty;
                this.Category = "Powders";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_5");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SGrass);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.SGameInstance.AssetDatabase.GetTexture("element_5");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 22;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 200)
            {
                this.Context.DestroyElement();
            }
        }
    }
}
