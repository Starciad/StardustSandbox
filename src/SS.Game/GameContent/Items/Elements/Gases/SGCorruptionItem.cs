using Microsoft.Xna.Framework;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Common.Utilities;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements.Common.Gases
{
    public sealed class SGCorruptionItem : SItem
    {
        public SGCorruptionItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_CORRUPTION_GAS";
            this.Name = "Corruption (Gas)";
            this.Description = string.Empty;
            this.Category = "Gases";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_16");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SGCorruption);
        }
    }
}
