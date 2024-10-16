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

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    [SGameContent]
    [SElementRegister(8)]
    [SItemRegister(typeof(SMCorruptionItem))]
    public sealed class SMCorruption : SMovableSolid
    {
        private sealed class SMCorruptionItem : SItem
        {
            public SMCorruptionItem(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance, assetDatabase)
            {
                this.Identifier = "ELEMENT_CORRUPTION_MOVABLE";
                this.Name = "Corruption (Movable)";
                this.Description = string.Empty;
                this.Category = "Powders";
                this.IconTexture = assetDatabase.GetTexture("icon_element_9");
                this.IsVisible = true;
                this.UnlockProgress = 0;
                this.ReferencedType = typeof(SMCorruption);
            }
        }

        public SMCorruption(SGame gameInstance) : base(gameInstance)
        {
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_9");
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