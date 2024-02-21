using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Common.Solid.Immovable;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Elements.Templates.Liquid;
using Microsoft.Xna.Framework;
using PixelDust.Game.World.Data;

using System;

namespace PixelDust.Game.Elements.Common.Liquid
{
    [PElementRegister(10)]
    public class PAcid : PLiquid
    {
        protected override void OnSettings()
        {
            this.Name = "Acid";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_11");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_11");

            this.Rendering.SetRenderingMechanism(new PElementBlobRenderingMechanism());

            this.DefaultTemperature = 10;

            this.EnableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Point, PWorldSlot)> neighbors, int length)
        {
            foreach ((Point, PWorldSlot) neighbor in neighbors)
            {
                if (this.Context.ElementDatabase.GetElementById(neighbor.Item2.Id) is PAcid ||
                    this.Context.ElementDatabase.GetElementById(neighbor.Item2.Id) is PWall)
                {
                    continue;
                }

                this.Context.DestroyElement();
                this.Context.DestroyElement(neighbor.Item1);
            }
        }
    }
}