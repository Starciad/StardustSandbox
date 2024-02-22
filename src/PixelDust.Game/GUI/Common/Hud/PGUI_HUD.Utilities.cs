using PixelDust.Game.Elements;

namespace PixelDust.Game.GUI.Common.HUD
{
    public sealed partial class PGUI_HUD
    {
        private int slotSelectedIndex = 0;

        private void SelectElementSlot(int slotIndex, int elementId)
        {
            this.slotSelectedIndex = slotIndex;
            this.Game.GameInputManager.SetSelectedElement(GetGameElement(elementId));
        }

        private PElement GetGameElement(int id)
        {
            return this.Game.ElementDatabase.GetElementById((uint)id);
        }
    }
}
