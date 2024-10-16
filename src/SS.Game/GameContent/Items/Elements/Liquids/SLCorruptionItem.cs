using Microsoft.Xna.Framework;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Common.Utilities;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements.Common.Liquid
{
    public sealed class SLCorruptionItem : SItem
    {
        public SLCorruptionItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_CORRUPTION_LIQUID";
            this.Name = "Corruption (Liquid)";
            this.Description = string.Empty;
            this.Category = "Liquids";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_17");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SLCorruption);
        }
    }
}
