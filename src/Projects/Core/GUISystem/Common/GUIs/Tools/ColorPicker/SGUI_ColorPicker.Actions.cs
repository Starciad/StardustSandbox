using Microsoft.Xna.Framework;

namespace StardustSandbox.Core.GUISystem.GUIs.Tools.ColorPicker
{
    internal sealed partial class SGUI_ColorPicker
    {
        private void CancelButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        private void SelectColorButtonAction(Color color)
        {
            this.SGameInstance.GUIManager.CloseGUI();
            this.colorPickerSettings?.OnSelectCallback?.Invoke(new(color));
        }
    }
}
