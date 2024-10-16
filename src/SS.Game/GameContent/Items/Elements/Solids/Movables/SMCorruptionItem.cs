using Microsoft.Xna.Framework;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Common.Utilities;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    public sealed class SMCorruptionItem : SItem
    {
        public SMCorruptionItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_CORRUPTION_MOVABLE";
            this.Name = "Corruption (Movable)";
            this.Description = string.Empty;
            this.Category = "Powders";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_9");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SMCorruption);
        }
    }
}