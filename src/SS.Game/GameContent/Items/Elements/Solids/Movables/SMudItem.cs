using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.Items.Elements.Solids.Movables
{
    public sealed class SMudItem : SItem
    {
        public SMudItem(SGame gameInstance) : base(gameInstance)
        {
            this.Identifier = "ELEMENT_MUD";
            this.Name = "Mud";
            this.Description = string.Empty;
            this.Category = "Powders";
            this.IconTexture = gameInstance.AssetDatabase.GetTexture("icon_element_2");
            this.IsVisible = true;
            this.UnlockProgress = 0;
            this.ReferencedType = typeof(SMud);
        }
    }
}
