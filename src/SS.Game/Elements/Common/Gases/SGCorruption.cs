using Microsoft.Xna.Framework;

using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Elements.Common.Utilities;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements.Common.Gases
{
    [SGameContent]
    [SElementRegister(15)]
    [SItemRegister(typeof(SGCorruptionItem))]
    public sealed class SGCorruption : SGas
    {
        private sealed class SGCorruptionItem : SItem
        {
            protected override void OnBuild()
            {
                this.Identifier = "ELEMENT_CORRUPTION_GAS";
                this.Name = "Corruption (Gas)";
                this.Description = string.Empty;
                this.Category = "Gases";
                this.IconTexture = this.AssetDatabase.GetTexture("icon_element_16");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SGCorruption);
            }
        }

        protected override void OnSettings()
        {
            this.Texture = this.Game.AssetDatabase.GetTexture("element_16");
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
