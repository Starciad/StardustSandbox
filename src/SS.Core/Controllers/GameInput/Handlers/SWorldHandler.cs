using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Handlers.Tools;
using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.Helpers;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Managers;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers
{
    internal sealed class SWorldHandler
    {
        private readonly ISGame gameInstance;

        private readonly SSimulationPlayer simulationPlayer;
        private readonly SSimulationPen simulationPen;

        private readonly SVisualizationTool visualizationTool;
        private readonly SPencilTool pencilTool;
        private readonly SEraserTool eraserTool;
        private readonly SFloodFillTool floodFillTool;
        private readonly SReplaceTool replaceTool;

        public SWorldHandler(ISGame gameInstance, SSimulationPlayer simulationPlayer, SSimulationPen simulationPen)
        {
            this.gameInstance = gameInstance;

            this.simulationPlayer = simulationPlayer;
            this.simulationPen = simulationPen;

            this.visualizationTool = new(this.gameInstance, simulationPen);
            this.pencilTool = new(this.gameInstance, simulationPen);
            this.eraserTool = new(this.gameInstance, simulationPen);
            this.floodFillTool = new(this.gameInstance, simulationPen);
            this.replaceTool = new(this.gameInstance, simulationPen);
        }

        public void Clear()
        {
            this.gameInstance.World.Clear();
        }

        public void Modify(SWorldModificationType worldModificationType)
        {
            if (!CanModifyWorld())
            {
                return;
            }

            Point mousePosition = GetWorldGridPositionFromMouse(this.gameInstance.InputManager, this.gameInstance.CameraManager).ToPoint();

            switch (this.simulationPen.Tool)
            {
                case SPenTool.Visualization:
                    this.visualizationTool.Execute(worldModificationType, this.simulationPlayer.SelectedItem.ContentType, this.simulationPlayer.SelectedItem.Identifier, mousePosition);
                    break;

                case SPenTool.Pencil:
                    this.pencilTool.Execute(worldModificationType, this.simulationPlayer.SelectedItem.ContentType, this.simulationPlayer.SelectedItem.Identifier, mousePosition);
                    break;

                case SPenTool.Eraser:
                    this.eraserTool.Execute(worldModificationType, this.simulationPlayer.SelectedItem.ContentType, this.simulationPlayer.SelectedItem.Identifier, mousePosition);
                    break;

                case SPenTool.Fill:
                    this.floodFillTool.Execute(worldModificationType, this.simulationPlayer.SelectedItem.ContentType, this.simulationPlayer.SelectedItem.Identifier, mousePosition);
                    break;

                case SPenTool.Replace:
                    this.replaceTool.Execute(worldModificationType, this.simulationPlayer.SelectedItem.ContentType, this.simulationPlayer.SelectedItem.Identifier, mousePosition);
                    break;

                default:
                    break;
            }
        }

        // ================================== //

        private bool CanModifyWorld()
        {
            return this.simulationPlayer.CanModifyEnvironment && this.simulationPlayer.SelectedItem != null;
        }

        private static Vector2 GetWorldGridPositionFromMouse(ISInputManager inputManager, ISCameraManager cameraManager)
        {
            return SWorldMath.ToWorldPosition(ConvertScreenToWorld(cameraManager, inputManager.GetScaledMousePosition()));
        }

        private static Vector2 ConvertScreenToWorld(ISCameraManager cameraManager, Vector2 screenPosition)
        {
            Vector3 screenPosition3D = new(screenPosition, 0);

            Matrix viewMatrix = cameraManager.GetViewMatrix();
            Matrix inverseViewMatrix = Matrix.Invert(viewMatrix);

            Vector3 worldPosition3D = Vector3.Transform(screenPosition3D, inverseViewMatrix);

            return new Vector2(worldPosition3D.X, worldPosition3D.Y);
        }
    }
}
