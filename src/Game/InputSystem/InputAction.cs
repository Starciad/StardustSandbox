using Microsoft.Xna.Framework.Input;

using StardustSandbox.Enums.Inputs;

namespace StardustSandbox.InputSystem
{
    internal sealed class InputAction
    {
        internal readonly struct CallbackContext(InputCallbackState state, MouseButton capturedMouseButton, Keys capturedKey)
        {
            internal readonly InputCallbackState State => state;
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

        private readonly Keys[] keys;
        private readonly MouseButton[] mouseButtons;

        internal InputAction(Keys[] keys)
        {
            this.keys = keys;
            this.mouseButtons = [];
        }

        internal InputAction(MouseButton[] mouseButtons)
        {
            this.keys = [];
            this.mouseButtons = mouseButtons;
        }

        internal InputAction(Keys[] keys, MouseButton[] mouseButtons)
        {
            this.keys = keys;
            this.mouseButtons = mouseButtons;
        }

        internal void Update()
        {
            this.capturedKey = Keys.None;
            this.capturedMouseButton = MouseButton.None;

            for (int i = 0; i < this.keys.Length; i++)
            {
                Keys key = this.keys[i];

                // Started
                if (GetKeyboardStartedState(key))
                {
                    this.capturedKey = key;
                    UpdateStarting();
                }

                // Performed
                if (GetKeyboardPerformedState(key))
                {
                    this.capturedKey = key;
                    UpdatePerformed();
                }

                // Canceled
                if (GetKeyboardCanceledState(key))
                {
                    this.capturedKey = key;
                    UpdateCanceled();
                }
            }

            for (int i = 0; i < this.mouseButtons.Length; i++)
            {
                MouseButton mouseButton = this.mouseButtons[i];

                // Started
                if (GetMouseStartedState(mouseButton) &&
                    !this.started && !this.performed && this.canceled)
                {
                    this.capturedMouseButton = mouseButton;
                    UpdateStarting();
                }

                // Performed
                if (GetMousePerformedState(mouseButton) &&
                    this.started && !this.canceled)
                {
                    this.capturedMouseButton = mouseButton;
                    UpdatePerformed();
                }

                // Canceled
                if (GetMouseCanceledState(mouseButton) &&
                    this.started && this.performed && !this.canceled)
                {
                    this.capturedMouseButton = mouseButton;
                    UpdateCanceled();
                }
            }
        }

        // Keyboard State
        private bool GetKeyboardStartedState(Keys key)
        {
            return !Input.PreviousKeyboardState.IsKeyDown(key) &&
                    Input.KeyboardState.IsKeyDown(key) &&
                   !this.started && !this.performed && this.canceled;
        }

        private bool GetKeyboardPerformedState(Keys key)
        {
            return Input.PreviousKeyboardState.IsKeyDown(key) &&
                   Input.KeyboardState.IsKeyDown(key) &&
                   this.started && !this.canceled;
        }

        private bool GetKeyboardCanceledState(Keys key)
        {
            return Input.PreviousKeyboardState.IsKeyDown(key) &&
                  !Input.KeyboardState.IsKeyDown(key) &&
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
                    previousState = Input.PreviousMouseState.LeftButton;
                    currentState = Input.MouseState.LeftButton;
                    break;

                case MouseButton.Middle:
                    previousState = Input.PreviousMouseState.MiddleButton;
                    currentState = Input.MouseState.MiddleButton;
                    break;

                case MouseButton.Right:
                    previousState = Input.PreviousMouseState.RightButton;
                    currentState = Input.MouseState.RightButton;
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
            OnStarted?.Invoke(new(InputCallbackState.Started, this.capturedMouseButton, this.capturedKey));

            this.started = true;
            this.canceled = false;
        }

        private void UpdatePerformed()
        {
            OnPerformed?.Invoke(new(InputCallbackState.Performed, this.capturedMouseButton, this.capturedKey));

            this.performed = true;
        }

        private void UpdateCanceled()
        {
            OnCanceled?.Invoke(new(InputCallbackState.Canceled, this.capturedMouseButton, this.capturedKey));

            this.started = false;
            this.performed = false;
            this.canceled = true;
        }
    }
}
