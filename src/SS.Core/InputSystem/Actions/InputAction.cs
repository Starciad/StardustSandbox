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
    internal sealed class InputAction(string name)
    {
        internal string Name => name;

        internal Keys KeyboardBinding { get; set; }
        internal MouseButton MouseBinding { get; set; }

        internal Started OnStarted { get; init; }
        internal Performed OnPerformed { get; init; }
        internal Canceled OnCanceled { get; init; }

        private bool started = false;
        private bool performed = false;
        private bool canceled = true;

        private Keys capturedKeyboardBinding;
        private MouseButton capturedMouseBinding;

        internal delegate void Started(InputCallbackContext context);
        internal delegate void Performed(InputCallbackContext context);
        internal delegate void Canceled(InputCallbackContext context);

        private bool IsKeyboardStarting(Keys key)
        {
            return !InputEngine.PreviousKeyboardState.IsKeyDown(key) &&
                    InputEngine.CurrentKeyboardState.IsKeyDown(key) &&
                   !this.started && !this.performed && this.canceled;
        }

        private bool IsKeyboardPerforming(Keys key)
        {
            return InputEngine.PreviousKeyboardState.IsKeyDown(key) &&
                   InputEngine.CurrentKeyboardState.IsKeyDown(key) &&
                   this.started && !this.canceled;
        }

        private bool IsKeyboardCanceling(Keys key)
        {
            return InputEngine.PreviousKeyboardState.IsKeyDown(key) &&
                  !InputEngine.CurrentKeyboardState.IsKeyDown(key) &&
                   this.started && this.performed && !this.canceled;
        }

        private static bool CheckMouseState(ButtonState previousState, ButtonState currentState, ButtonState desiredState)
        {
            return previousState is ButtonState.Pressed && currentState == desiredState;
        }

        private static bool GetMouseState(MouseButton mouseButton, ButtonState desiredState)
        {
            ButtonState previousState, currentState;

            switch (mouseButton)
            {
                case MouseButton.Left:
                    previousState = InputEngine.PreviousMouseState.LeftButton;
                    currentState = InputEngine.CurrentMouseState.LeftButton;
                    break;

                case MouseButton.Middle:
                    previousState = InputEngine.PreviousMouseState.MiddleButton;
                    currentState = InputEngine.CurrentMouseState.MiddleButton;
                    break;

                case MouseButton.Right:
                    previousState = InputEngine.PreviousMouseState.RightButton;
                    currentState = InputEngine.CurrentMouseState.RightButton;
                    break;

                default:
                    return false;
            }

            return CheckMouseState(previousState, currentState, desiredState);
        }

        private static bool IsMouseStarting(MouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Pressed);
        }

        private static bool IsMousePerforming(MouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Pressed);
        }

        private static bool IsMouseCanceling(MouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Released);
        }

        private void UpdateStarting()
        {
            this.OnStarted?.Invoke(new(InputState.Started, this.capturedMouseBinding, this.capturedKeyboardBinding));

            this.started = true;
            this.canceled = false;
        }

        private void UpdatePerformed()
        {
            this.OnPerformed?.Invoke(new(InputState.Performed, this.capturedMouseBinding, this.capturedKeyboardBinding));

            this.performed = true;
        }

        private void UpdateCanceled()
        {
            this.OnCanceled?.Invoke(new(InputState.Canceled, this.capturedMouseBinding, this.capturedKeyboardBinding));

            this.started = false;
            this.performed = false;
            this.canceled = true;
        }

        private void HandleKeyboardInput()
        {
            if (this.KeyboardBinding is Keys.None)
            {
                return;
            }

            // Started
            if (IsKeyboardStarting(this.KeyboardBinding))
            {
                this.capturedKeyboardBinding = this.KeyboardBinding;
                UpdateStarting();
            }

            // Performed
            if (IsKeyboardPerforming(this.KeyboardBinding))
            {
                this.capturedKeyboardBinding = this.KeyboardBinding;
                UpdatePerformed();
            }

            // Canceled
            if (IsKeyboardCanceling(this.KeyboardBinding))
            {
                this.capturedKeyboardBinding = this.KeyboardBinding;
                UpdateCanceled();
            }
        }

        private void HandleMouseInput()
        {
            if (this.MouseBinding is MouseButton.None)
            {
                return;
            }

            // Started
            if (IsMouseStarting(this.MouseBinding) &&
                !this.started && !this.performed && this.canceled)
            {
                this.capturedMouseBinding = this.MouseBinding;
                UpdateStarting();
            }

            // Performed
            if (IsMousePerforming(this.MouseBinding) &&
                this.started && !this.canceled)
            {
                this.capturedMouseBinding = this.MouseBinding;
                UpdatePerformed();
            }

            // Canceled
            if (IsMouseCanceling(this.MouseBinding) &&
                this.started && this.performed && !this.canceled)
            {
                this.capturedMouseBinding = this.MouseBinding;
                UpdateCanceled();
            }
        }

        internal void Update()
        {
            this.capturedKeyboardBinding = Keys.None;
            this.capturedMouseBinding = MouseButton.None;

            HandleKeyboardInput();
            HandleMouseInput();
        }
    }
}
