using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(14)]
    internal sealed class Wall : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Wall";
            Description = string.Empty;
            
            Render.AddFrame(new(3, 1));

            EnableTemperature = false;
        }
    }
}
