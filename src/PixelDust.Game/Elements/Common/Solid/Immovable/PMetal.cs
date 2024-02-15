using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PElementRegister(12)]
    public sealed class PMetal : PImmovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Metal";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_1");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_13");

            this.DefaultTemperature = 30;
        }
    }
}
