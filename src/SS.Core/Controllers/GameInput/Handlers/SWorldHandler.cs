using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Gameplay;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.World;

using System;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers
{
    internal sealed class SWorldHandler(SWorld world, SInputManager inputManager, SCameraManager cameraManager, SSimulationPlayer simulationPlayer, SSimulationPen simulationPen, SElementDatabase elementDatabase)
    {
        private readonly SWorld world = world;

        private readonly SInputManager inputManager = inputManager;
        private readonly SCameraManager cameraManager = cameraManager;

        private readonly SSimulationPlayer simulationPlayer = simulationPlayer;
        private readonly SSimulationPen simulationPen = simulationPen;

        private readonly SElementDatabase elementDatabase = elementDatabase;

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
            Point mouseWorldPosition = GetWorldGridPositionFromMouse();

            switch (worldModificationType)
            {
                case SWorldModificationType.Adding:
                    AddItems(itemType, mouseWorldPosition);
                    break;

                case SWorldModificationType.Removing:
                    RemoveItems(mouseWorldPosition);
                    break;

                default:
                    break;
            }
        }

        private void AddItems(Type itemType, Point position)
        {
            if (typeof(SElement).IsAssignableFrom(itemType))
            {
                AddElements(this.elementDatabase.GetElementByType(itemType), position);
            }
        }

        private void RemoveItems(Point position)
        {
            RemoveElements(position);
        }

        // ========================= //

        private void AddElements(ISElement element, Point position)
        {
            if (!this.world.InsideTheWorldDimensions(position))
            {
                return;
            }

            ApplyPenAction(position, (position) => this.world.InstantiateElement(new Point(position.X, position.Y), element.Id));
        }

        private void RemoveElements(Point position)
        {
            if (!this.world.InsideTheWorldDimensions(position))
            {
                return;
            }

            ApplyPenAction(position, this.world.DestroyElement);
        }


        // ================================== //
        // Utilities

        private void ApplyPenAction(Point centerPos, Action<Point> action)
        {
            for (int x = -this.simulationPen.Size; x < this.simulationPen.Size; x++)
            {
                for (int y = -this.simulationPen.Size; y < this.simulationPen.Size; y++)
                {
                    Point localPos = new Point(x, y) + centerPos;

                    if (this.world.InsideTheWorldDimensions(localPos))
                    {
                        action.Invoke(new(localPos.X, localPos.Y));
                    }
                }
            }
        }

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
