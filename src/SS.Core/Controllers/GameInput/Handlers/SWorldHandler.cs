using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Handlers.Tools;
using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.Managers;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.World;

using System;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers
{
    internal sealed class SWorldHandler
    {
        private readonly ISWorld world;

        private readonly ISInputManager inputManager;
        private readonly ISCameraManager cameraManager;

        private readonly ISElementDatabase elementDatabase;

        private readonly SSimulationPlayer simulationPlayer;
        private readonly SSimulationPen simulationPen;

        private readonly SPencilTool pencilTool;
        private readonly SFloodFillTool floodFillTool;
        private readonly SReplaceTool replaceTool;

        public SWorldHandler(ISWorld world, ISInputManager inputManager, ISCameraManager cameraManager, SSimulationPlayer simulationPlayer, SSimulationPen simulationPen, ISElementDatabase elementDatabase)
        {
            this.world = world;

            this.inputManager = inputManager;
            this.cameraManager = cameraManager;
            this.elementDatabase = elementDatabase;

            this.simulationPlayer = simulationPlayer;
            this.simulationPen = simulationPen;

            this.pencilTool = new(this.world, this.elementDatabase, simulationPen);
            this.floodFillTool = new(this.world, this.elementDatabase, simulationPen);
            this.replaceTool = new(this.world, this.elementDatabase, simulationPen);
        }

        public void Clear()
        {
            this.world.Clear();
        }

        public void Modify(SWorldModificationType worldModificationType)
        {
            if (!CanModifyWorld())
            {
                return;
            }

            Type itemType = this.simulationPlayer.SelectedItem.ReferencedType;
            Point mousePosition = GetWorldGridPositionFromMouse();

            switch (this.simulationPen.Tool)
            {
                case SPenTool.Pencil:
                    this.pencilTool.Execute(worldModificationType, itemType, mousePosition);
                    break;

                case SPenTool.Fill:
                    this.floodFillTool.Execute(worldModificationType, itemType, mousePosition);
                    break;

                case SPenTool.Replace:
                    this.replaceTool.Execute(worldModificationType, itemType, mousePosition);
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
            Vector2 mousePosition = this.inputManager.GetScaledMousePosition();

            Vector2 worldPosition = ConvertScreenToWorld(mousePosition);
            Vector2 gridPosition = SWorld.ToWorldPosition(worldPosition);

            return gridPosition.ToPoint();
        }

        private Vector2 ConvertScreenToWorld(Vector2 screenPosition)
        {
            Vector3 screenPosition3D = new(screenPosition, 0);

            Matrix viewMatrix = this.cameraManager.GetViewMatrix();
            Matrix inverseViewMatrix = Matrix.Invert(viewMatrix);

            Vector3 worldPosition3D = Vector3.Transform(screenPosition3D, inverseViewMatrix);

            return new Vector2(worldPosition3D.X, worldPosition3D.Y);
        }
    }
}
