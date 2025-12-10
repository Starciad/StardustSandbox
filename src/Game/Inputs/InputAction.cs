using Microsoft.Xna.Framework.Input;

using StardustSandbox.Enums.Inputs;
using StardustSandbox.Managers;

namespace StardustSandbox.Inputs
{
    internal sealed class InputAction
    {
        internal InputActionMap ActionMap => this.actionMap;

        private InputActionMap actionMap;

        private CallbackContext callback;

        private bool started = false;
        private bool performed = false;
        private bool canceled = true;

        internal event Started OnStarted;
        internal event Performed OnPerformed;
        internal event Canceled OnCanceled;

        internal delegate void Started(CallbackContext context);
        internal delegate void Performed(CallbackContext context);
        internal delegate void Canceled(CallbackContext context);

        private readonly Keys[] keys;
        private readonly MouseButton[] mouseButtons;
        private readonly InputManager inputManager;

        internal InputAction(InputManager inputManager, params Keys[] keys)
        {
            this.inputManager = inputManager;
            this.keys = keys;
            this.mouseButtons = [];
        }

        internal InputAction(InputManager inputManager, params MouseButton[] mouseButtons)
        {
            this.inputManager = inputManager;
            this.keys = [];
            this.mouseButtons = mouseButtons;
        }

        internal InputAction(InputManager inputManager, Keys[] keys, MouseButton[] mouseButtons)
        {
            this.inputManager = inputManager;
            this.keys = keys;
            this.mouseButtons = mouseButtons;
        }

        internal void Update()
        {
            this.callback = new();

            foreach (Keys key in this.keys)
            {
                // Started
                if (GetKeyboardStartedState(key))
                {
                    this.callback.CapturedKey = key;
                    UpdateStarting();
                }

                // Performed
                if (GetKeyboardPerformedState(key))
                {
                    this.callback.CapturedKey = key;
                    UpdatePerformed();
                }

                // Canceled
                if (GetKeyboardCanceledState(key))
                {
                    this.callback.CapturedKey = key;
                    UpdateCanceled();
                }
            }

            foreach (MouseButton mouseButton in this.mouseButtons)
            {
                // Started
                if (GetMouseStartedState(mouseButton) &&
                    !this.started && !this.performed && this.canceled)
                {
                    this.callback.CapturedMouseButton = mouseButton;
                    UpdateStarting();
                }

                // Performed
                if (GetMousePerformedState(mouseButton) &&
                    this.started && !this.canceled)
                {
                    this.callback.CapturedMouseButton = mouseButton;
                    UpdatePerformed();
                }

                // Canceled
                if (GetMouseCanceledState(mouseButton) &&
                    this.started && this.performed && !this.canceled)
                {
                    this.callback.CapturedMouseButton = mouseButton;
                    UpdateCanceled();
                }
            }
        }

        internal void SetActionMap(InputActionMap map)
        {
            this.actionMap = map;
        }

        // Keyboard State
        private bool GetKeyboardStartedState(Keys key)
        {
            return !this.inputManager.PreviousKeyboardState.IsKeyDown(key) &&
                    this.inputManager.KeyboardState.IsKeyDown(key) &&
                   !this.started && !this.performed && this.canceled;
        }
        private bool GetKeyboardPerformedState(Keys key)
        {
            return this.inputManager.PreviousKeyboardState.IsKeyDown(key) &&
                   this.inputManager.KeyboardState.IsKeyDown(key) &&
                   this.started && !this.canceled;
        }
        private bool GetKeyboardCanceledState(Keys key)
        {
            return this.inputManager.PreviousKeyboardState.IsKeyDown(key) &&
                  !this.inputManager.KeyboardState.IsKeyDown(key) &&
                   this.started && this.performed && !this.canceled;
        }

        // Mouse State
        private bool GetMouseStartedState(MouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Pressed);
        }
        private bool GetMousePerformedState(MouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Pressed);
        }
        private bool GetMouseCanceledState(MouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Released);
        }

        private bool GetMouseState(MouseButton mouseButton, ButtonState desiredState)
        {
            ButtonState previousState, currentState;

            switch (mouseButton)
            {
                case MouseButton.Left:
                    previousState = this.inputManager.PreviousMouseState.LeftButton;
                    currentState = this.inputManager.MouseState.LeftButton;
                    break;
                case MouseButton.Middle:
                    previousState = this.inputManager.PreviousMouseState.MiddleButton;
                    currentState = this.inputManager.MouseState.MiddleButton;
                    break;
                case MouseButton.Right:
                    previousState = this.inputManager.PreviousMouseState.RightButton;
                    currentState = this.inputManager.MouseState.RightButton;
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

        // Update
        private void UpdateStarting()
        {
            this.callback.State = InputCallbackState.Started;

            OnStarted?.Invoke(this.callback);

            this.started = true;
            this.canceled = false;
        }
        private void UpdatePerformed()
        {
            this.callback.State = InputCallbackState.Performed;

            OnPerformed?.Invoke(this.callback);

            this.performed = true;
        }
        private void UpdateCanceled()
        {
            this.callback.State = InputCallbackState.Canceled;

            OnCanceled?.Invoke(this.callback);

            this.started = false;
            this.performed = false;
            this.canceled = true;
        }

        internal struct CallbackContext
        {
            internal InputCallbackState State { get; set; }
            internal MouseButton CapturedMouseButton { get; set; }
            internal Keys CapturedKey { get; set; }
        }
    }
}
