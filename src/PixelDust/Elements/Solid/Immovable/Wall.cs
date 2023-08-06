using PixelDust.Core;
using Microsoft.Xna.Framework;

namespace PixelDust
{
    [PElementRegister]
    internal sealed class Wall : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Wall";
            Description = string.Empty;
            Color = Color.Gray;
        }
    }
}
