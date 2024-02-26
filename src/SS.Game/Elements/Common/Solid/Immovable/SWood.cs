using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Immovable
{
    [SGameContent]
    [SElementRegister(14)]
    [SItemRegister(typeof(SWoodItem))]
    public sealed class SWood : SImmovableSolid
    {
        private sealed class SWoodItem : SItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_WOOD";
                this.Name = "Wood";
                this.Description = string.Empty;
                this.Category = "Solids";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_15");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SWood);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_15");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
        }
    }
}
