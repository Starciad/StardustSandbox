using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Enums.Gameplay;
using StardustSandbox.Game.Enums.General;
using StardustSandbox.Game.Enums.InputSystem;
using StardustSandbox.Game.InputSystem;

using System;

namespace StardustSandbox.Game.Managers
{
    public sealed partial class SGameInputManager
    {
        private readonly SInputActionMapHandler _actionHandler = new();
        private readonly SInputManager _inputHandler = inputHandler;

        // ================================== //

        private void BuildKeyboardInputs()
        {
            SInputActionMap worldKeyboardActionMap = this._actionHandler.AddActionMap("World_Keyboard", true);

            // Camera
            worldKeyboardActionMap.AddAction("World_Camera_Up", new SInputAction(this._inputHandler, Keys.W, Keys.Up)).OnPerformed += _ => MoveCamera(SCardinalDirection.North);
            worldKeyboardActionMap.AddAction("World_Camera_Right", new SInputAction(this._inputHandler, Keys.D, Keys.Right)).OnPerformed += _ => MoveCamera(SCardinalDirection.East);
            worldKeyboardActionMap.AddAction("World_Camera_Down", new SInputAction(this._inputHandler, Keys.S, Keys.Down)).OnPerformed += _ => MoveCamera(SCardinalDirection.South);
            worldKeyboardActionMap.AddAction("World_Camera_Left", new SInputAction(this._inputHandler, Keys.A, Keys.Left)).OnPerformed += _ => MoveCamera(SCardinalDirection.West);

            // Shortcuts
            worldKeyboardActionMap.AddAction("World_Pause", new(this._inputHandler, Keys.Space)).OnStarted += _ => PauseWorld();
            worldKeyboardActionMap.AddAction("World_Reset", new(this._inputHandler, Keys.R)).OnStarted += _ => ResetWorld();
        }
        private void BuildMouseInputs()
        {
            SInputActionMap worldMouseActionMap = this._actionHandler.AddActionMap("World_Mouse", true);

            worldMouseActionMap.AddAction("World_Place_Elements", new(this._inputHandler, SMouseButton.Left)).OnPerformed += _ => PerformMapAction(SMapActionType.Put);
            worldMouseActionMap.AddAction("World_Erase_Elements", new(this._inputHandler, SMouseButton.Right)).OnPerformed += _ => PerformMapAction(SMapActionType.Remove);
        }

        // ================================== //
        // Camera
        private void MoveCamera(SCardinalDirection direction)
        {
            switch (direction)
            {
                case SCardinalDirection.North:
                    cameraManager.Move(new Vector2(0, this.cameraMovementSpeed));
                    break;

                case SCardinalDirection.East:
                    cameraManager.Move(new Vector2(this.cameraMovementSpeed, 0));
                    break;

                case SCardinalDirection.South:
                    cameraManager.Move(new Vector2(0, -this.cameraMovementSpeed));
                    break;

                case SCardinalDirection.West:
                    cameraManager.Move(new Vector2(-this.cameraMovementSpeed, 0));
                    break;

                default:
                    return;
            }
        }

        // ================================== //
        // World
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

        // ================================== //
        // Perform
        private void PerformMapAction(SMapActionType mapActionType)
        {
            if (!this.canModifyEnvironment || this.itemSelected == null)
            {
                return;
            }

            if (typeof(SElement).IsAssignableFrom(this.itemSelected.ReferencedType))
            {
                // ====================================================== //
                // The currently selected item corresponds to an element.
                // ====================================================== //

                if (mapActionType == SMapActionType.Put)
                {
                    PutElementsInWorld(this.Game.ElementDatabase.GetElementByType(this.itemSelected.ReferencedType));
                }
                else if (mapActionType == SMapActionType.Remove)
                {
                    RemoveElementsFromWorld();
                }
            }
        }

        #region ELEMENTS
        private void PutElementsInWorld(SElement element)
        {
            Point worldPos = GetWorldPositionFromMouse().ToPoint();

            if (!IsValidWorldPosition(worldPos))
            {
                return;
            }

            if (this.penScale == 0)
            {
                world.InstantiateElement(worldPos, element.Id);
                return;
            }

            ApplyPenAction(worldPos, (int x, int y) => world.InstantiateElement(new Point(x, y), element.Id));
        }
        private void RemoveElementsFromWorld()
        {
            Point worldPos = GetWorldPositionFromMouse().ToPoint();

            if (!IsValidWorldPosition(worldPos))
            {
                return;
            }

            if (this.penScale == 0)
            {
                world.DestroyElement(worldPos);
                return;
            }

            ApplyPenAction(worldPos, (x, y) => world.DestroyElement(new Point(x, y)));
        }
        private bool IsValidWorldPosition(Point worldPos)
        {
            return world.InsideTheWorldDimensions(worldPos);
        }
        private void ApplyPenAction(Point centerPos, Action<int, int> action)
        {
            for (int x = -this.penScale; x < this.penScale; x++)
            {
                for (int y = -this.penScale; y < this.penScale; y++)
                {
                    Point localPos = new Point(x, y) + centerPos;

                    if (world.InsideTheWorldDimensions(localPos))
                    {
                        action.Invoke(localPos.X, localPos.Y);
                    }
                }
            }
        }
        #endregion

        // ================================== //
        // Utilities
        private Vector2 GetWorldPositionFromMouse()
        {
            Vector2 screenPos = cameraManager.ScreenToWorld(this._inputHandler.MouseState.Position.ToVector2());
            return new Vector2(screenPos.X, screenPos.Y) / SWorldConstants.GRID_SCALE;
        }
    }
}
