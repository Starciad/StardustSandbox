using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.GUISystem.GUIs.Hud
{
    public sealed partial class SGUI_HUD
    {
        private void SelectItemSlot(int slotIndex, string itemId)
        {
            this.slotSelectedIndex = slotIndex;
            this.SGameInstance.GameInputManager.SelectItem(GetGameItemById(itemId));
        }

        private SItem GetGameItemByIndex(int index)
        {
            return this.SGameInstance.ItemDatabase.Items[index];
        }

        private SItem GetGameItemById(string id)
        {
            return this.SGameInstance.ItemDatabase.GetItemById(id);
        }
    }
}
