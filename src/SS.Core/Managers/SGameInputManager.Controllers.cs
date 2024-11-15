using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Gameplay;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Enums.InputSystem;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces.Elements;

using System;

namespace StardustSandbox.Core.Managers
{
    public sealed partial class SGameInputManager
    {
        private void BuildKeyboardInputs()
        {
            SInputActionMap worldKeyboardActionMap = this._actionHandler.AddActionMap("World_Keyboard", true);

            // Camera
            worldKeyboardActionMap.AddAction("World_Camera_Up", new SInputAction(this.SGameInstance, this._inputManager, Keys.W, Keys.Up)).OnPerformed += _ => MoveCamera(SCardinalDirection.North);
            worldKeyboardActionMap.AddAction("World_Camera_Right", new SInputAction(this.SGameInstance, this._inputManager, Keys.D, Keys.Right)).OnPerformed += _ => MoveCamera(SCardinalDirection.East);
            worldKeyboardActionMap.AddAction("World_Camera_Down", new SInputAction(this.SGameInstance, this._inputManager, Keys.S, Keys.Down)).OnPerformed += _ => MoveCamera(SCardinalDirection.South);
            worldKeyboardActionMap.AddAction("World_Camera_Left", new SInputAction(this.SGameInstance, this._inputManager, Keys.A, Keys.Left)).OnPerformed += _ => MoveCamera(SCardinalDirection.West);
            worldKeyboardActionMap.AddAction("World_Camera_Zoom_In", new SInputAction(this.SGameInstance, this._inputManager, Keys.OemPlus)).OnPerformed += _ => CameraZoomIn();
            worldKeyboardActionMap.AddAction("World_Camera_Zoom_Out", new SInputAction(this.SGameInstance, this._inputManager, Keys.OemMinus)).OnPerformed += _ => CameraZoomOut();

            // Shortcuts
            worldKeyboardActionMap.AddAction("World_Pause", new(this.SGameInstance, this._inputManager, Keys.Space)).OnStarted += _ => PauseWorld();
            worldKeyboardActionMap.AddAction("World_Reset", new(this.SGameInstance, this._inputManager, Keys.R)).OnStarted += _ => ResetWorld();
        }
        private void BuildMouseInputs()
        {
            SInputActionMap worldMouseActionMap = this._actionHandler.AddActionMap("World_Mouse", true);

            worldMouseActionMap.AddAction("World_Place_Elements", new(this.SGameInstance, this._inputManager, SMouseButton.Left)).OnPerformed += _ => PerformMapAction(SMapActionType.Put);
            worldMouseActionMap.AddAction("World_Erase_Elements", new(this.SGameInstance, this._inputManager, SMouseButton.Right)).OnPerformed += _ => PerformMapAction(SMapActionType.Remove);
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

        private void CameraZoomIn()
        {
            this._cameraManager.ZoomIn(0.005f);
        }

        private void CameraZoomOut()
        {
            this._cameraManager.ZoomOut(0.005f);
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
        private void PutElementsInWorld(ISElement element)
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
