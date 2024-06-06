using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GUI.Common.HUD
{
    public sealed partial class SGUI_HUD
    {
        private void SelectItemSlot(int slotIndex, string itemId)
        {
            this.slotSelectedIndex = slotIndex;
            this.Game.GameInputManager.SelectItem(GetGameItemById(itemId));
        }

        private SItem GetGameItemByIndex(int index)
        {
            return this.Game.ItemDatabase.Items[index];
        }

        private SItem GetGameItemById(string id)
        {
            return this.Game.ItemDatabase.GetItemById(id);
        }
    }
}
