using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PixelDust.Game.Constants;
using PixelDust.Game.Elements;
using PixelDust.Game.Enums.Gameplay;
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

            worldMouseActionMap.AddAction("World_Place_Elements", new(this._inputHandler, PMouseButton.Left)).OnPerformed += _ => PerformMapAction(PMapActionType.Put);
            worldMouseActionMap.AddAction("World_Erase_Elements", new(this._inputHandler, PMouseButton.Right)).OnPerformed += _ => PerformMapAction(PMapActionType.Remove);
        }

        // ================================== //
        // Camera
        private void MoveCamera(PCardinalDirection direction)
        {
            switch (direction)
            {
                case PCardinalDirection.North:
                    cameraManager.Move(new Vector2(0, this.cameraMovementSpeed));
                    break;

                case PCardinalDirection.East:
                    cameraManager.Move(new Vector2(this.cameraMovementSpeed, 0));
                    break;

                case PCardinalDirection.South:
                    cameraManager.Move(new Vector2(0, -this.cameraMovementSpeed));
                    break;

                case PCardinalDirection.West:
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
        private void PerformMapAction(PMapActionType mapActionType)
        {
            if (!this.canModifyEnvironment || this.itemSelected == null)
            {
                return;
            }

            if (typeof(PElement).IsAssignableFrom(this.itemSelected.ReferencedType))
            {
                // ====================================================== //
                // The currently selected item corresponds to an element.
                // ====================================================== //

                if (mapActionType == PMapActionType.Put)
                {
                    PutElementsInWorld(this.Game.ElementDatabase.GetElementByType(this.itemSelected.ReferencedType));
                }
                else if (mapActionType == PMapActionType.Remove)
                {
                    RemoveElementsFromWorld();
                }
            }
        }

        #region ELEMENTS
        private void PutElementsInWorld(PElement element)
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
            return new Vector2(screenPos.X, screenPos.Y) / PWorldConstants.GRID_SCALE;
        }
    }
}
