﻿using PixelDust.Core.Elements;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister]
    internal class Petroleum : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Petroleum";
            Description = string.Empty;
            Color = new(31, 23, 23);
        }
    }
}