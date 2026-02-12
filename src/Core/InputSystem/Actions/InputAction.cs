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

using Microsoft.Xna.Framework.Input;

using StardustSandbox.Core.Enums.Inputs;

namespace StardustSandbox.Core.InputSystem.Actions
{
    internal sealed class InputAction
    {
        internal readonly struct CallbackContext(InputState state, MouseButton capturedMouseButton, Keys capturedKey)
        {
            internal readonly InputState State => state;
            internal readonly MouseButton CapturedMouseButton => capturedMouseButton;
            internal readonly Keys CapturedKey => capturedKey;
        }

        internal Started OnStarted { get; init; }
        internal Performed OnPerformed { get; init; }
        internal Canceled OnCanceled { get; init; }

        private bool started = false;
        private bool performed = false;
        private bool canceled = true;

        private Keys capturedKey;
        private MouseButton capturedMouseButton;

        internal delegate void Started(CallbackContext context);
        internal delegate void Performed(CallbackContext context);
        internal delegate void Canceled(CallbackContext context);

        private readonly Keys key;
        private readonly MouseButton mouseButton;

        internal InputAction(Keys value)
        {
            this.key = value;
            this.mouseButton = MouseButton.None;
        }

        internal InputAction(MouseButton value)
        {
            this.key = Keys.None;
            this.mouseButton = value;
        }

        internal void Update()
        {
            this.capturedKey = Keys.None;
            this.capturedMouseButton = MouseButton.None;

            // Keyboard State
            if (this.key != Keys.None)
            {
                // Started
                if (GetKeyboardStartedState(this.key))
                {
                    this.capturedKey = this.key;
                    UpdateStarting();
                }

                // Performed
                if (GetKeyboardPerformedState(this.key))
                {
                    this.capturedKey = this.key;
                    UpdatePerformed();
                }

                // Canceled
                if (GetKeyboardCanceledState(this.key))
                {
                    this.capturedKey = this.key;
                    UpdateCanceled();
                }
            }

            // Mouse State
            if (this.mouseButton != MouseButton.None)
            {
                // Started
                if (GetMouseStartedState(this.mouseButton) &&
                    !this.started && !this.performed && this.canceled)
                {
                    this.capturedMouseButton = this.mouseButton;
                    UpdateStarting();
                }

                // Performed
                if (GetMousePerformedState(this.mouseButton) &&
                    this.started && !this.canceled)
                {
                    this.capturedMouseButton = this.mouseButton;
                    UpdatePerformed();
                }

                // Canceled
                if (GetMouseCanceledState(this.mouseButton) &&
                    this.started && this.performed && !this.canceled)
                {
                    this.capturedMouseButton = this.mouseButton;
                    UpdateCanceled();
                }
            }
        }

        // Keyboard State
        private bool GetKeyboardStartedState(Keys key)
        {
            return !InputEngine.PreviousKeyboardState.IsKeyDown(key) &&
                    InputEngine.KeyboardState.IsKeyDown(key) &&
                   !this.started && !this.performed && this.canceled;
        }

        private bool GetKeyboardPerformedState(Keys key)
        {
            return InputEngine.PreviousKeyboardState.IsKeyDown(key) &&
                   InputEngine.KeyboardState.IsKeyDown(key) &&
                   this.started && !this.canceled;
        }

        private bool GetKeyboardCanceledState(Keys key)
        {
            return InputEngine.PreviousKeyboardState.IsKeyDown(key) &&
                  !InputEngine.KeyboardState.IsKeyDown(key) &&
                   this.started && this.performed && !this.canceled;
        }

        // Mouse State
        private static bool GetMouseStartedState(MouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Pressed);
        }

        private static bool GetMousePerformedState(MouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Pressed);
        }

        private static bool GetMouseCanceledState(MouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Released);
        }

        private static bool GetMouseState(MouseButton mouseButton, ButtonState desiredState)
        {
            ButtonState previousState, currentState;

            switch (mouseButton)
            {
                case MouseButton.Left:
                    previousState = InputEngine.PreviousMouseState.LeftButton;
                    currentState = InputEngine.MouseState.LeftButton;
                    break;

                case MouseButton.Middle:
                    previousState = InputEngine.PreviousMouseState.MiddleButton;
                    currentState = InputEngine.MouseState.MiddleButton;
                    break;

                case MouseButton.Right:
                    previousState = InputEngine.PreviousMouseState.RightButton;
                    currentState = InputEngine.MouseState.RightButton;
                    break;

                default:
                    return false;
            }

            return CheckMouseState(previousState, currentState, desiredState);
        }

        private static bool CheckMouseState(ButtonState previousState, ButtonState currentState, ButtonState desiredState)
        {
            return previousState == ButtonState.Pressed && currentState == desiredState;
        }

        private void UpdateStarting()
        {
            this.OnStarted?.Invoke(new(InputState.Started, this.capturedMouseButton, this.capturedKey));

            this.started = true;
            this.canceled = false;
        }

        private void UpdatePerformed()
        {
            this.OnPerformed?.Invoke(new(InputState.Performed, this.capturedMouseButton, this.capturedKey));

            this.performed = true;
        }

        private void UpdateCanceled()
        {
            this.OnCanceled?.Invoke(new(InputState.Canceled, this.capturedMouseButton, this.capturedKey));

            this.started = false;
            this.performed = false;
            this.canceled = true;
        }
    }
}
