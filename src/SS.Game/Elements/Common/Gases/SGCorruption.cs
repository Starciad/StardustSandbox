using Microsoft.Xna.Framework;

using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Databases;
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
            public SGCorruptionItem(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance, assetDatabase)
            {
                this.Identifier = "ELEMENT_CORRUPTION_GAS";
                this.Name = "Corruption (Gas)";
                this.Description = string.Empty;
                this.Category = "Gases";
                this.IconTexture = assetDatabase.GetTexture("icon_element_16");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SGCorruption);
            }
        }

        public SGCorruption(SGame gameInstance) : base(gameInstance)
        {
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_16");
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
