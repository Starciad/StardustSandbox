using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PElementRegister(13)]
    public sealed class PWall : PImmovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Wall";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_1");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_14");

            this.EnableTemperature = false;
        }
    }
}
