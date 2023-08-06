using PixelDust.Core.Elements;

using Microsoft.Xna.Framework;

namespace PixelDust.Game.Elements.Solid.Immovable
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
