using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Handlers.Tools;
using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers
{
    internal sealed class SWorldHandler
    {
        private readonly ISGame game;

        private readonly SSimulationPlayer simulationPlayer;
        private readonly SSimulationPen simulationPen;

        private readonly SVisualizationTool visualizationTool;
        private readonly SPencilTool pencilTool;
        private readonly SEraserTool eraserTool;
        private readonly SFloodFillTool floodFillTool;
        private readonly SReplaceTool replaceTool;

        public SWorldHandler(ISGame game, SSimulationPlayer simulationPlayer, SSimulationPen simulationPen)
        {
            this.game = game;

            this.simulationPlayer = simulationPlayer;
            this.simulationPen = simulationPen;

            this.visualizationTool = new(this.game, simulationPen);
            this.pencilTool = new(this.game, simulationPen);
            this.eraserTool = new(this.game, simulationPen);
            this.floodFillTool = new(this.game, simulationPen);
            this.replaceTool = new(this.game, simulationPen);
        }

        public void Clear()
        {
            this.game.World.Clear();
        }

        public void Modify(SWorldModificationType worldModificationType)
        {
            if (!CanModifyWorld())
            {
                return;
            }

            Point mousePosition = GetWorldGridPositionFromMouse();

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

        private Point GetWorldGridPositionFromMouse()
        {
            Vector2 mousePosition = this.game.InputManager.GetScaledMousePosition();

            Vector2 worldPosition = ConvertScreenToWorld(mousePosition);
            Vector2 gridPosition = SWorldMath.ToWorldPosition(worldPosition);

            return gridPosition.ToPoint();
        }

        private Vector2 ConvertScreenToWorld(Vector2 screenPosition)
        {
            Vector3 screenPosition3D = new(screenPosition, 0);

            Matrix viewMatrix = this.game.CameraManager.GetViewMatrix();
            Matrix inverseViewMatrix = Matrix.Invert(viewMatrix);

            Vector3 worldPosition3D = Vector3.Transform(screenPosition3D, inverseViewMatrix);

            return new Vector2(worldPosition3D.X, worldPosition3D.Y);
        }
    }
}
