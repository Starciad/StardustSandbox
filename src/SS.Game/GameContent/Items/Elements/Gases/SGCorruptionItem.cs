using StardustSandbox.Game.GameContent.Elements.Gases;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Gases
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
