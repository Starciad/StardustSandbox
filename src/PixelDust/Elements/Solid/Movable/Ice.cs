using PixelDust.Core;

using Microsoft.Xna.Framework;

namespace PixelDust
{
    [PElementRegister]
    internal sealed class Ice : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Ice";
            Description = string.Empty;
            Color = new Color(63, 208, 212);
        }
    }
}
