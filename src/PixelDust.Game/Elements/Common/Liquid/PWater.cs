using Microsoft.Xna.Framework;

using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Gases;
using PixelDust.Game.Elements.Common.Solid.Movable;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Elements.Templates.Liquid;
using PixelDust.Game.Tools;
using PixelDust.Game.World.Data;

using System;

namespace PixelDust.Game.Elements.Common.Liquid
{
    [PElementRegister(2)]
    public class PWater : PLiquid
    {
        protected override void OnSettings()
        {
            this.Name = "Water";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_3");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_3");

            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());

            this.DefaultDispersionRate = 3;
            this.DefaultTemperature = 25;

            this.EnableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Point, PWorldSlot)> neighbors, int length)
        {
            foreach ((Point, PWorldSlot) neighbor in neighbors)
            {
                if (this.Context.ElementDatabase.GetElementById(neighbor.Item2.Id) is PDirt)
                {
                    this.Context.DestroyElement();
                    this.Context.ReplaceElement<PMud>(neighbor.Item1);
                    return;
                }

                if (this.Context.ElementDatabase.GetElementById(neighbor.Item2.Id) is PStone)
                {
                    if (PRandom.Range(0, 150) == 0)
                    {
                        this.Context.DestroyElement();
                        this.Context.ReplaceElement<PSand>(neighbor.Item1);
                        return;
                    }
                }
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 100)
            {
                this.Context.ReplaceElement<PSteam>();
            }

            if (currentValue <= 0)
            {
                this.Context.ReplaceElement<PIce>();
            }
        }
    }
}