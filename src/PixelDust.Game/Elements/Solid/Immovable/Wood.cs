using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister]
    internal sealed class Wood : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Wood";
            Description = string.Empty;
            Color = new(43, 24, 12);
        }
    }
}
