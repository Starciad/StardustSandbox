using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Solid.Immovable
{
    [PElementRegister]
    internal sealed class Glass : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Glass";
            Description = string.Empty;
            Color = new(252, 249, 243);
        }
    }
}