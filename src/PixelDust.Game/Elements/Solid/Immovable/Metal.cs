using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister]
    internal sealed class Metal : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Metal";
            Description = string.Empty;
            Color = new(80, 80, 80);
        }
    }
}
