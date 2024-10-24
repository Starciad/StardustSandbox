﻿using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.GameContent.Elements.Gases;
using StardustSandbox.Game.GameContent.Elements.Rendering;
using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.GameContent.Elements.Liquids
{
    public class SWater : SLiquid
    {
        public SWater(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 002;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_3");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultDispersionRate = 3;
            this.DefaultTemperature = 25;
            this.EnableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Point, SWorldSlot)> neighbors, int length)
        {
            foreach ((Point, SWorldSlot) neighbor in neighbors)
            {
                if (neighbor.Item2.Element is SDirt)
                {
                    this.Context.DestroyElement();
                    this.Context.ReplaceElement<SMud>(neighbor.Item1);
                    return;
                }

                if (neighbor.Item2.Element is SStone)
                {
                    if (SRandomMath.Range(0, 150) == 0)
                    {
                        this.Context.DestroyElement();
                        this.Context.ReplaceElement<SSand>(neighbor.Item1);
                        return;
                    }
                }
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 100)
            {
                this.Context.ReplaceElement<SSteam>();
            }

            if (currentValue <= 0)
            {
                this.Context.ReplaceElement<SIce>();
            }
        }
    }
}