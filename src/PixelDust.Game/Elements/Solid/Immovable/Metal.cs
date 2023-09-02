using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister(12)]
    internal sealed class Metal : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Metal";
            Description = string.Empty;

            
            Render.AddFrame(new(2, 1));
        }
    }
}
