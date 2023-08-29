﻿using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;

namespace PixelDust.Game.Elements.Solid.Movable
{
    [PElementRegister(4)]
    internal sealed class Stone : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Stone";
            Description = string.Empty;

            Render = new();
            Render.AddFrame(new(3, 0));
        }
    }
}
