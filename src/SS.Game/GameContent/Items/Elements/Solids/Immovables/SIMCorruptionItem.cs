using Microsoft.Xna.Framework;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Common.Utilities;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements.Common.Solid.Immovable
{
    public sealed class SIMCorruptionItem : SItem
    {
        public SIMCorruptionItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_CORRUPTION_IMMOVABLE";
            this.Name = "Corruption (Immovable)";
            this.Description = string.Empty;
            this.Category = "Solids";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_18");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SIMCorruption);
        }
    }
}
