using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(15)]
    internal sealed class Wood : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Wood";
            Description = string.Empty;

            Render.AddFrame(new(4, 1));

            DefaultTemperature = 20;
        }
    }
}
