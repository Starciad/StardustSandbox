using Microsoft.Xna.Framework;

using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Elements.Common.Utilities;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements.Common.Liquid
{
    [SGameContent]
    [SElementRegister(16)]
    [SItemRegister(typeof(SLCorruptionItem))]
    public class SLCorruption : SLiquid
    {
        private sealed class SLCorruptionItem : SItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_CORRUPTION_LIQUID";
                this.Name = "Corruption (Liquid)";
                this.Description = string.Empty;
                this.Category = "Liquids";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_17");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SLCorruption);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.SGameInstance.AssetDatabase.GetTexture("element_17");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.EnableNeighborsAction = true;
        }

        protected override void OnStep()
        {
            if (this.Context.TryGetElementNeighbors(out ReadOnlySpan<(Point, SWorldSlot)> neighbors))
            {
                this.Context.InfectNeighboringElements(neighbors, neighbors.Length);
            }
        }
    }
}
