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

            this.Render.AddFrame(new(3, 1));

            this.EnableTemperature = false;
        }
    }
}
