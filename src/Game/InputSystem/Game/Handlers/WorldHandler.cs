using Microsoft.Xna.Framework;

using StardustSandbox.Camera;
using StardustSandbox.Enums.Inputs;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.InputSystem.Game.Handlers.Gizmos;
using StardustSandbox.InputSystem.Game.Simulation;
using StardustSandbox.Managers;
using StardustSandbox.Mathematics;
using StardustSandbox.Tools;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.InputSystem.Game.Handlers
{
    internal sealed class WorldHandler
    {
        internal ToolContext ToolContext { get; private set; }

        private readonly Player player;
        private readonly Pen pen;

        private readonly VisualizationGizmo visualizationGizmo;
        private readonly PencilGizmo pencilGizmo;
        private readonly EraserGizmo eraserGizmo;
        private readonly FloodFillGizmo floodFillGizmo;
        private readonly ReplaceGizmo replaceGizmo;

        private readonly World world;

        internal WorldHandler(ActorManager actorManager, Pen pen, Player player, World world)
        {
            this.world = world;

            this.ToolContext = new(world);

            this.player = player;
            this.pen = pen;

            this.visualizationGizmo = new(actorManager, pen, world, this);
            this.pencilGizmo = new(actorManager, pen, world, this);
            this.eraserGizmo = new(actorManager, pen, world, this);
            this.floodFillGizmo = new(actorManager, pen, world, this);
            this.replaceGizmo = new(actorManager, pen, world, this);
        }

        internal void Modify(in WorldModificationType worldModificationType, in InputState inputState)
        {
            if (!CanModifyWorld())
            {
                return;
            }

            Point mousePosition = GetWorldGridPositionFromMouse().ToPoint();

            switch (this.pen.Tool)
            {
                case PenTool.Visualization:
                    this.visualizationGizmo.Execute(worldModificationType, inputState, this.player.SelectedItem.ContentType, this.player.SelectedItem.ContentIndex, mousePosition);
                    break;

                case PenTool.Pencil:
                    this.pencilGizmo.Execute(worldModificationType, inputState, this.player.SelectedItem.ContentType, this.player.SelectedItem.ContentIndex, mousePosition);
                    break;

                case PenTool.Eraser:
                    this.eraserGizmo.Execute(worldModificationType, inputState, this.player.SelectedItem.ContentType, this.player.SelectedItem.ContentIndex, mousePosition);
                    break;

                case PenTool.Fill:
                    this.floodFillGizmo.Execute(worldModificationType, inputState, this.player.SelectedItem.ContentType, this.player.SelectedItem.ContentIndex, mousePosition);
                    break;

                case PenTool.Replace:
                    this.replaceGizmo.Execute(worldModificationType, inputState, this.player.SelectedItem.ContentType, this.player.SelectedItem.ContentIndex, mousePosition);
                    break;

                default:
                    break;
            }
        }

        private bool CanModifyWorld()
        {
            return this.player.CanModifyEnvironment && this.player.SelectedItem != null;
        }

        private static Vector2 GetWorldGridPositionFromMouse()
        {
            return WorldMath.ToWorldPosition(ConvertScreenToWorld(Input.GetScaledMousePosition()));
        }

        private static Vector2 ConvertScreenToWorld(Vector2 screenPosition)
        {
            Vector3 worldPosition3D = Vector3.Transform(new(screenPosition, 0), Matrix.Invert(SSCamera.GetViewMatrix()));

            return new Vector2(worldPosition3D.X, worldPosition3D.Y);
        }
    }
}
