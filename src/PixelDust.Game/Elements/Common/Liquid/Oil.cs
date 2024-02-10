﻿using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Liquid;

namespace PixelDust.Game.Elements.Common.Liquid
{
    [PElementRegister(21)]
    public class Oil : PLiquid
    {
        protected override void OnSettings()
        {
            this.Name = "Oil";
            this.Description = string.Empty;

            this.DefaultTemperature = 30;
        }
    }
}