using Microsoft.Xna.Framework;

using StardustSandbox.Camera;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Inputs.Game.Handlers.Gizmos;
using StardustSandbox.Inputs.Game.Simulation;
using StardustSandbox.Interfaces.Tools;
using StardustSandbox.Managers;
using StardustSandbox.Mathematics;
using StardustSandbox.Tools;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Inputs.Game.Handlers
{
    internal sealed class WorldHandler
    {
        internal IToolContext ToolContext { get; private set; }

        private readonly Player player;
        private readonly Pen pen;

        private readonly VisualizationGizmo visualizationGizmo;
        private readonly PencilGizmo pencilGizmo;
        private readonly EraserGizmo eraserGizmo;
        private readonly FloodFillGizmo floodFillGizmo;
        private readonly ReplaceGizmo replaceGizmo;

        private readonly InputManager inputManager;

        private readonly World world;

        internal WorldHandler(InputManager inputManager, Player player, Pen pen, World world)
        {
            this.world = world;

            this.inputManager = inputManager;

            this.ToolContext = new ToolContext(world);

            this.player = player;
            this.pen = pen;

            this.visualizationGizmo = new(pen, world, this);
            this.pencilGizmo = new(pen, world, this);
            this.eraserGizmo = new(pen, world, this);
            this.floodFillGizmo = new(pen, world, this);
            this.replaceGizmo = new(pen, world, this);
        }

        internal void Clear()
        {
            this.world.Clear();
        }

        internal void Modify(WorldModificationType worldModificationType)
        {
            if (!CanModifyWorld())
            {
                return;
            }

            Point mousePosition = GetWorldGridPositionFromMouse(this.inputManager).ToPoint();

            switch (this.pen.Tool)
            {
                case PenTool.Visualization:
                    this.visualizationGizmo.Execute(worldModificationType, this.player.SelectedItem.ContentType, this.player.SelectedItem.AssociatedType, mousePosition);
                    break;

                case PenTool.Pencil:
                    this.pencilGizmo.Execute(worldModificationType, this.player.SelectedItem.ContentType, this.player.SelectedItem.AssociatedType, mousePosition);
                    break;

                case PenTool.Eraser:
                    this.eraserGizmo.Execute(worldModificationType, this.player.SelectedItem.ContentType, this.player.SelectedItem.AssociatedType, mousePosition);
                    break;

                case PenTool.Fill:
                    this.floodFillGizmo.Execute(worldModificationType, this.player.SelectedItem.ContentType, this.player.SelectedItem.AssociatedType, mousePosition);
                    break;

                case PenTool.Replace:
                    this.replaceGizmo.Execute(worldModificationType, this.player.SelectedItem.ContentType, this.player.SelectedItem.AssociatedType, mousePosition);
                    break;

                default:
                    break;
            }
        }

        // ================================== //

        private bool CanModifyWorld()
        {
            return this.player.CanModifyEnvironment && this.player.SelectedItem != null;
        }

        private static Vector2 GetWorldGridPositionFromMouse(InputManager inputManager)
        {
            return WorldMath.ToWorldPosition(ConvertScreenToWorld(inputManager.GetScaledMousePosition()));
        }

        private static Vector2 ConvertScreenToWorld(Vector2 screenPosition)
        {
            Vector3 worldPosition3D = Vector3.Transform(new(screenPosition, 0), Matrix.Invert(SSCamera.GetViewMatrix()));

            return new Vector2(worldPosition3D.X, worldPosition3D.Y);
        }
    }
}
