using PixelDust.Game.Items;

namespace PixelDust.Game.GUI.Common.HUD
{
    public sealed partial class PGUI_HUD
    {
        private void SelectItemSlot(int slotIndex, string itemId)
        {
            this.slotSelectedIndex = slotIndex;
            this.Game.GameInputManager.SelectItem(GetGameItemById(itemId));
        }

        private PItem GetGameItemByIndex(int index)
        {
            return this.Game.ItemDatabase.Items[index];
        }

        private PItem GetGameItemById(string id)
        {
            return this.Game.ItemDatabase.GetItemById(id);
        }
    }
}
