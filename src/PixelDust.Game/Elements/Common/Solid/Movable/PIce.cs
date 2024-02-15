using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(5)]
    public sealed class PIce : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Ice";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_1");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_6");

            this.DefaultTemperature = 0;
        }
    }
}
