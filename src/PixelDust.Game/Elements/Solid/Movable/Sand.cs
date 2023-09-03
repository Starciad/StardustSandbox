using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(5)]
    internal sealed class Sand : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Sand";
            Description = string.Empty;
            
            Render.AddFrame(new(6, 0));

            DefaultTemperature = 22;
        }
    }
}
