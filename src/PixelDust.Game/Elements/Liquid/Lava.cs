using PixelDust.Core.Elements;
using PixelDust.Core.Mathematics;
using PixelDust.Core.Worlding;

using PixelDust.Game.Elements.Gases;
using PixelDust.Game.Elements.Solid.Movable;

using System;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister(15)]
    internal class Lava : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Lava";
            Description = string.Empty;
            
            Render.AddFrame(new(9, 0));

            DefaultTemperature = 1000;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue < 500)
            {
                Context.TryReplace<Stone>(Context.Position);
                Context.TrySetTemperature(Context.Position, 550);
            }
        }
    }
}