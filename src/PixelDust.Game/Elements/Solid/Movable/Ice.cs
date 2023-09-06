using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(6)]
    internal sealed class Ice : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Ice";
            Description = string.Empty;
            
            Render.AddFrame(new(5, 0));

            DefaultTemperature = 0;
        }
    }
}
