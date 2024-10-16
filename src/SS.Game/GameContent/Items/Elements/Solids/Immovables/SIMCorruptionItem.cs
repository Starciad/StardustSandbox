using StardustSandbox.Game.GameContent.Elements.Solids.Immovables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Solids.Immovables
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
