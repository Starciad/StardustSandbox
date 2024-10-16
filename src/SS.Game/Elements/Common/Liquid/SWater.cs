using Microsoft.Xna.Framework;

using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Common.Gases;
using StardustSandbox.Game.Elements.Common.Solid.Movable;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.General;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements.Common.Liquid
{
    [SGameContent]
    [SElementRegister(2)]
    [SItemRegister(typeof(SWaterItem))]
    public class SWater : SLiquid
    {
        private sealed class SWaterItem : SItem
        {
            public SWaterItem(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance, assetDatabase)
            {
                this.Identifier = "ELEMENT_WATER";
                this.Name = "Water";
                this.Description = string.Empty;
                this.Category = "Liquids";
                this.IconTexture = assetDatabase.GetTexture("icon_element_3");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SWater);
            }
        }

        public SWater(SGame gameInstance) : base(gameInstance)
        {
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
                if (this.Context.ElementDatabase.GetElementById(neighbor.Item2.Id) is SDirt)
                {
                    this.Context.DestroyElement();
                    this.Context.ReplaceElement<SMud>(neighbor.Item1);
                    return;
                }

                if (this.Context.ElementDatabase.GetElementById(neighbor.Item2.Id) is SStone)
                {
                    if (SRandom.Range(0, 150) == 0)
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