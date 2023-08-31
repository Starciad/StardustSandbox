using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister(17)]
    internal class Oil : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Oil";
            Description = string.Empty;

            
        }
    }
}
