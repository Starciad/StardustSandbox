using Microsoft.Xna.Framework;

using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Elements.Common.Solid.Immovable;
using PixelDust.Game.Elements.Rendering.Common;
using PixelDust.Game.Items;
using PixelDust.Game.World.Data;

using System;

namespace PixelDust.Game.Elements.Common.Liquid
{
    [PGameContent]
    [PElementRegister(10)]
    [PItemRegister(typeof(PAcidItem))]
    public class PAcid : PLiquid
    {
        private sealed class PAcidItem : PItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_ACID";
                this.Name = "Acid";
                this.Description = string.Empty;
                this.Category = string.Empty;
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_11");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(PAcid);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_11");
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