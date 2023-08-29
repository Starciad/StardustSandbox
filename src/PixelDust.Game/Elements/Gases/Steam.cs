using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Gases
{
    [PElementRegister(19)]
    internal class Steam : PGas
    {
        protected override void OnSettings()
        {
            Name = "Steam";
            Description = string.Empty;

            Render = new();
            Render.AddFrame(new(9, 1));
        }
    }
}
