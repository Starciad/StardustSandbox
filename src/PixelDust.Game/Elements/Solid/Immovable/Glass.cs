using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(12)]
    internal sealed class Glass : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Glass";
            Description = string.Empty;

            Render.AddFrame(new(1, 1));

            DefaultTemperature = 25;
        }
    }
}