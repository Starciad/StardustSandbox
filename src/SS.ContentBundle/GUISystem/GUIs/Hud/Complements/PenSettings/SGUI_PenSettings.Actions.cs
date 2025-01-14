using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.Enums.World;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.PenSettings
{
    internal sealed partial class SGUI_PenSettings
    {
        // Menu
        private void ExitButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        // Tools
        private void SelectToolButtonAction(SPenTool tool)
        {
            this.gameInputController.Pen.Tool = tool;
        }

        // Layers
        private void SelectLayerButtonAction(SWorldLayer layer)
        {
            this.gameInputController.Pen.Layer = layer;
        }

        // Shapes
        private void SelectShapeButtonAction(SPenShape shape)
        {
            this.gameInputController.Pen.Shape = shape;
        }
    }
}
