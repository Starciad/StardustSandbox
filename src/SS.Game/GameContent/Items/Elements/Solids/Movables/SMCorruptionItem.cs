using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Solids.Movables
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