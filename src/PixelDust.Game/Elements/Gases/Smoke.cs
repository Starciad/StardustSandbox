using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Gases
{
    [PElementRegister(18)]
    internal class Smoke : PGas
    {
        protected override void OnSettings()
        {
            Name = "Smoke";
            Description = string.Empty;

            // Render
            Render.AddFrame(new(9, 1));
        }
    }
}
