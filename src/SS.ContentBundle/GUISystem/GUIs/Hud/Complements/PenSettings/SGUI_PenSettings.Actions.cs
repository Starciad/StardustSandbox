using StardustSandbox.Core.Enums.Gameplay.Pen;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_PenSettings
    {
        // Menu
        private void ExitButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        // Tools
        private void SelectPencilToolButtonAction()
        {
            this.gameInputController.Pen.Tool = SPenTool.Pencil;
        }

        private void SelectFillToolButtonAction()
        {
            this.gameInputController.Pen.Tool = SPenTool.Fill;
        }

        private void SelectReplaceToolButtonAction()
        {
            this.gameInputController.Pen.Tool = SPenTool.Replace;
        }

        // Layers
        private void SelectFrontLayerButtonAction()
        {
            this.gameInputController.Pen.Layer = SPenLayer.Front;
        }

        private void SelectBackLayerButtonAction()
        {
            this.gameInputController.Pen.Layer = SPenLayer.Back;
        }
    }
}
