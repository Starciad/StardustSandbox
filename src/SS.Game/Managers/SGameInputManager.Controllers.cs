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

        // ================================== //

        private void BuildKeyboardInputs()
        {
            SInputActionMap worldKeyboardActionMap = this._actionHandler.AddActionMap("World_Keyboard", true);

            // Camera
            worldKeyboardActionMap.AddAction("World_Camera_Up", new SInputAction(this._inputManager, Keys.W, Keys.Up)).OnPerformed += _ => MoveCamera(SCardinalDirection.North);
            worldKeyboardActionMap.AddAction("World_Camera_Right", new SInputAction(this._inputManager, Keys.D, Keys.Right)).OnPerformed += _ => MoveCamera(SCardinalDirection.East);
            worldKeyboardActionMap.AddAction("World_Camera_Down", new SInputAction(this._inputManager, Keys.S, Keys.Down)).OnPerformed += _ => MoveCamera(SCardinalDirection.South);
            worldKeyboardActionMap.AddAction("World_Camera_Left", new SInputAction(this._inputManager, Keys.A, Keys.Left)).OnPerformed += _ => MoveCamera(SCardinalDirection.West);

            // Shortcuts
            worldKeyboardActionMap.AddAction("World_Pause", new(this._inputManager, Keys.Space)).OnStarted += _ => PauseWorld();
            worldKeyboardActionMap.AddAction("World_Reset", new(this._inputManager, Keys.R)).OnStarted += _ => ResetWorld();
        }
        private void BuildMouseInputs()
        {
            SInputActionMap worldMouseActionMap = this._actionHandler.AddActionMap("World_Mouse", true);

            worldMouseActionMap.AddAction("World_Place_Elements", new(this._inputManager, SMouseButton.Left)).OnPerformed += _ => PerformMapAction(SMapActionType.Put);
            worldMouseActionMap.AddAction("World_Erase_Elements", new(this._inputManager, SMouseButton.Right)).OnPerformed += _ => PerformMapAction(SMapActionType.Remove);
        }

        // ================================== //
        // Camera
        private void MoveCamera(SCardinalDirection direction)
        {
            switch (direction)
            {
                case SCardinalDirection.North:
                    this._cameraManager.Move(new Vector2(0, this.cameraMovementSpeed));
                    break;

                case SCardinalDirection.East:
                    this._cameraManager.Move(new Vector2(this.cameraMovementSpeed, 0));
                    break;

                case SCardinalDirection.South:
                    this._cameraManager.Move(new Vector2(0, -this.cameraMovementSpeed));
                    break;

                case SCardinalDirection.West:
                    this._cameraManager.Move(new Vector2(-this.cameraMovementSpeed, 0));
                    break;

                default:
                    return;
            }
        }

        // ================================== //
        // World
        private void PauseWorld()
        {
            if (this._world.States.IsPaused)
            {
                this._world.Resume();
            }
            else
            {
                this._world.Pause();
            }
        }
        private void ResetWorld()
        {
            this._world.Clear();
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
                    PutElementsInWorld(this.SGameInstance.ElementDatabase.GetElementByType(this.itemSelected.ReferencedType));
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
                this._world.InstantiateElement(worldPos, element.Id);
                return;
            }

            ApplyPenAction(worldPos, (int x, int y) => this._world.InstantiateElement(new Point(x, y), element.Id));
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
                this._world.DestroyElement(worldPos);
                return;
            }

            ApplyPenAction(worldPos, (x, y) => this._world.DestroyElement(new Point(x, y)));
        }
        private bool IsValidWorldPosition(Point worldPos)
        {
            return this._world.InsideTheWorldDimensions(worldPos);
        }
        private void ApplyPenAction(Point centerPos, Action<int, int> action)
        {
            for (int x = -this.penScale; x < this.penScale; x++)
            {
                for (int y = -this.penScale; y < this.penScale; y++)
                {
                    Point localPos = new Point(x, y) + centerPos;

                    if (this._world.InsideTheWorldDimensions(localPos))
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
            Vector2 screenPos = this._cameraManager.ScreenToWorld(this._inputManager.MouseState.Position.ToVector2());
            return new Vector2(screenPos.X, screenPos.Y) / SWorldConstants.GRID_SCALE;
        }
    }
}
