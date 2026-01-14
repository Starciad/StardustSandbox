/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;

using StardustSandbox.Core;
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
            Vector3 worldPosition3D = Vector3.Transform(new(screenPosition, 0), Matrix.Invert(Camera.GetViewMatrix()));

            return new(worldPosition3D.X, worldPosition3D.Y);
        }
    }
}

