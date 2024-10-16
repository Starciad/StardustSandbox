using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    [SGameContent]
    [SElementRegister(0)]
    [SItemRegister(typeof(SDirtItem))]
    public sealed class SDirt : SMovableSolid
    {
        private sealed class SDirtItem : SItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_DIRT";
                this.Name = "Dirt";
                this.Description = string.Empty;
                this.Category = "Powders";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_1");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SDirt);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.SGameInstance.AssetDatabase.GetTexture("element_1");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
            this.EnableNeighborsAction = true;
        }
    }
}
