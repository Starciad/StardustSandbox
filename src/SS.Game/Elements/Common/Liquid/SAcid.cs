﻿using Microsoft.Xna.Framework;

using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Elements.Common.Solid.Immovable;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements.Common.Liquid
{
    [SGameContent]
    [SElementRegister(10)]
    [SItemRegister(typeof(SAcidItem))]
    public class SAcid : SLiquid
    {
        private sealed class SAcidItem : SItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_ACID";
                this.Name = "Acid";
                this.Description = string.Empty;
                this.Category = "Liquids";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_11");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SAcid);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_11");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 10;
            this.EnableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Point, SWorldSlot)> neighbors, int length)
        {
            foreach ((Point, SWorldSlot) neighbor in neighbors)
            {
                if (this.Context.ElementDatabase.GetElementById(neighbor.Item2.Id) is SAcid ||
                    this.Context.ElementDatabase.GetElementById(neighbor.Item2.Id) is SWall)
                {
                    continue;
                }

                this.Context.DestroyElement();
                this.Context.DestroyElement(neighbor.Item1);
            }
        }
    }
}