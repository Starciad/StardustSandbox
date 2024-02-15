using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PixelDust.Game.Constants;
using PixelDust.Game.Enums.General;
using PixelDust.Game.Enums.InputSystem;
using PixelDust.Game.InputSystem;

using System;

namespace PixelDust.Game.Managers
{
    public sealed partial class PGameInputManager
    {
        private readonly PInputActionMapHandler _actionHandler = new();
        private readonly PInputManager _inputHandler = inputHandler;

        // ================================== //

        private void BuildKeyboardInputs()
        {
            PInputActionMap worldKeyboardActionMap = this._actionHandler.AddActionMap("World_Keyboard", true);

            // Camera
            worldKeyboardActionMap.AddAction("World_Camera_Up", new PInputAction(this._inputHandler, Keys.W, Keys.Up)).OnPerformed += _ => MoveCamera(PCardinalDirection.North);
            worldKeyboardActionMap.AddAction("World_Camera_Right", new PInputAction(this._inputHandler, Keys.D, Keys.Right)).OnPerformed += _ => MoveCamera(PCardinalDirection.East);
            worldKeyboardActionMap.AddAction("World_Camera_Down", new PInputAction(this._inputHandler, Keys.S, Keys.Down)).OnPerformed += _ => MoveCamera(PCardinalDirection.South);
            worldKeyboardActionMap.AddAction("World_Camera_Left", new PInputAction(this._inputHandler, Keys.A, Keys.Left)).OnPerformed += _ => MoveCamera(PCardinalDirection.West);

            // Shortcuts
            worldKeyboardActionMap.AddAction("World_Pause", new(this._inputHandler, Keys.Space)).OnStarted += _ => PauseWorld();
            worldKeyboardActionMap.AddAction("World_Reset", new(this._inputHandler, Keys.R)).OnStarted += _ => ResetWorld();
        }
        private void BuildMouseInputs()
        {
            PInputActionMap worldMouseActionMap = this._actionHandler.AddActionMap("World_Mouse", true);

            worldMouseActionMap.AddAction("World_Place_Elements", new(this._inputHandler, PMouseButton.Left)).OnPerformed += _ => PlaceElementsInWorld();
            worldMouseActionMap.AddAction("World_Erase_Elements", new(this._inputHandler, PMouseButton.Right)).OnPerformed += _ => DeleteElementsInWorld();
        }

        // ================================== //

        private void MoveCamera(PCardinalDirection direction)
        {
            switch (direction)
            {
                case PCardinalDirection.North:
                    orthographicCamera.Move(new Vector2(0, this.cameraMovementSpeed));
                    break;

                case PCardinalDirection.East:
                    orthographicCamera.Move(new Vector2(this.cameraMovementSpeed, 0));
                    break;

                case PCardinalDirection.South:
                    orthographicCamera.Move(new Vector2(0, -this.cameraMovementSpeed));
                    break;

                case PCardinalDirection.West:
                    orthographicCamera.Move(new Vector2(-this.cameraMovementSpeed, 0));
                    break;

                default:
                    return;
            }
        }

        private void PauseWorld()
        {
            if (world.States.IsPaused)
            {
                world.Resume();
            }
            else
            {
                world.Pause();
            }
        }
        private void ResetWorld()
        {
            world.Clear();
        }

        private void PlaceElementsInWorld()
        {
            if (!this.canModifyEnvironment)
            {
                return;
            }

            Vector2 worldPos = GetWorldPositionFromMouse();

            if (!IsValidWorldPosition(worldPos) || this.elementSelected == null)
            {
                return;
            }

            UpdateElementOver(worldPos);

            if (this.penScale == 0)
            {
                world.InstantiateElement(worldPos, this.elementSelected.Id);
                return;
            }

            ApplyPenAction(worldPos, (x, y) => world.InstantiateElement(new Vector2(x, y), this.elementSelected.Id));
        }
        private void DeleteElementsInWorld()
        {
            if (!this.canModifyEnvironment)
            {
                return;
            }

            Vector2 worldPos = GetWorldPositionFromMouse();

            if (!IsValidWorldPosition(worldPos) || this.elementSelected == null)
            {
                return;
            }

            UpdateElementOver(worldPos);

            if (this.penScale == 0)
            {
                world.DestroyElement(worldPos);
                return;
            }

            ApplyPenAction(worldPos, (x, y) => world.DestroyElement(new Vector2(x, y)));
        }

        private Vector2 GetWorldPositionFromMouse()
        {
            Vector2 screenPos = orthographicCamera.ScreenToWorld(this._inputHandler.MouseState.Position.ToVector2());
            return new Vector2(screenPos.X, screenPos.Y) / PWorldConstants.GRID_SCALE;
        }

        private bool IsValidWorldPosition(Vector2 worldPos)
        {
            return world.InsideTheWorldDimensions(worldPos) && this.elementSelected != null;
        }

        private void UpdateElementOver(Vector2 worldPos)
        {
            this.elementOver = world.GetElement(worldPos);
        }

        private void ApplyPenAction(Vector2 centerPos, Action<int, int> action)
        {
            for (int x = -this.penScale; x < this.penScale; x++)
            {
                for (int y = -this.penScale; y < this.penScale; y++)
                {
                    Vector2 localPos = new Vector2(x, y) + centerPos;

                    if (world.InsideTheWorldDimensions(localPos))
                    {
                        action.Invoke((int)localPos.X, (int)localPos.Y);
                    }
                }
            }
        }
    }
}
