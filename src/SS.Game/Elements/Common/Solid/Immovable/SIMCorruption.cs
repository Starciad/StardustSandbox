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

namespace StardustSandbox.Game.Elements.Common.Solid.Immovable
{
    [SGameContent]
    [SElementRegister(17)]
    [SItemRegister(typeof(SIMCorruptionItem))]
    public class SIMCorruption : SImmovableSolid
    {
        private sealed class SIMCorruptionItem : SItem
        {
            public SIMCorruptionItem(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance, assetDatabase)
            {
                this.Identifier = "ELEMENT_CORRUPTION_IMMOVABLE";
                this.Name = "Corruption (Immovable)";
                this.Description = string.Empty;
                this.Category = "Solids";
                this.IconTexture = assetDatabase.GetTexture("icon_element_18");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SIMCorruption);
            }
        }

        public SIMCorruption(SGame gameInstance) : base(gameInstance)
        {
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_18");
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
