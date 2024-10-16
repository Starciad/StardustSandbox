using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Immovable
{
    [SGameContent]
    [SElementRegister(13)]
    [SItemRegister(typeof(SWallItem))]
    public sealed class SWall : SImmovableSolid
    {
        private sealed class SWallItem : SItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_WALL";
                this.Name = "Wall";
                this.Description = string.Empty;
                this.Category = "Solids";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_14");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SWall);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.SGameInstance.AssetDatabase.GetTexture("element_14");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.EnableTemperature = false;
        }
    }
}
