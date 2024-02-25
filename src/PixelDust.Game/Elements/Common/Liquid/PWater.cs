using Microsoft.Xna.Framework;

using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Common.Gases;
using PixelDust.Game.Elements.Common.Solid.Movable;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.General;
using PixelDust.Game.Items;
using PixelDust.Game.World.Data;

using System;

namespace PixelDust.Game.Elements.Common.Liquid
{
    [PGameContent]
    [PElementRegister(2)]
    [PItemRegister(typeof(PWaterItem))]
    public class PWater : PLiquid
    {
        private sealed class PWaterItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_WATER";
                this.Name = "Water";
                this.Description = string.Empty;
                this.Category = string.Empty;
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_3");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PWater);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_3");
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