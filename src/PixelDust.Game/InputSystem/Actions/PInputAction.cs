using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PixelDust.Game.InputSystem.Enums;
using PixelDust.Game.Managers;
using PixelDust.Game.Objects;

namespace PixelDust.Game.InputSystem.Actions
{
    public sealed class PInputAction : PGameObject
    {
        public PInputActionMap ActionMap => this._actionMap;

        private PInputActionMap _actionMap;
        private readonly PInputManager _inputManager;

        private readonly Keys[] keys;
        private readonly PMouseButton[] mouseButtons;

        private CallbackContext callback;

        private bool started = false;
        private bool performed = false;
        private bool canceled = true;

        public event Started OnStarted;
        public event Performed OnPerformed;
        public event Canceled OnCanceled;

        public delegate void Started(CallbackContext context);
        public delegate void Performed(CallbackContext context);
        public delegate void Canceled(CallbackContext context);

        public PInputAction(PInputManager inputManager, params Keys[] keys)
        {
            this._inputManager = inputManager;
            this.keys = keys;
            this.mouseButtons = [];
        }
        public PInputAction(PInputManager inputManager, params PMouseButton[] mouseButtons)
        {
            this._inputManager = inputManager;
            this.keys = [];
            this.mouseButtons = mouseButtons;
        }
        public PInputAction(PInputManager inputManager, Keys[] keys, PMouseButton[] mouseButtons)
        {
            this._inputManager = inputManager;
            this.keys = keys;
            this.mouseButtons = mouseButtons;
        }

        protected override void OnUpdate(GameTime gameTime)
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

            foreach (PMouseButton mouseButton in this.mouseButtons)
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

        internal void SetActionMap(PInputActionMap map)
        {
            this._actionMap = map;
        }

        // Keyboard State
        private bool GetKeyboardStartedState(Keys key)
        {
            return !this._inputManager.PreviousKeyboardState.IsKeyDown(key) &&
                    this._inputManager.KeyboardState.IsKeyDown(key) &&
                   !this.started && !this.performed && this.canceled;
        }
        private bool GetKeyboardPerformedState(Keys key)
        {
            return this._inputManager.PreviousKeyboardState.IsKeyDown(key) &&
                   this._inputManager.KeyboardState.IsKeyDown(key) &&
                   this.started && !this.canceled;
        }
        private bool GetKeyboardCanceledState(Keys key)
        {
            return this._inputManager.PreviousKeyboardState.IsKeyDown(key) &&
                  !this._inputManager.KeyboardState.IsKeyDown(key) &&
                   this.started && this.performed && !this.canceled;
        }

        // Mouse State
        private bool GetMouseStartedState(PMouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Pressed);
        }
        private bool GetMousePerformedState(PMouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Pressed);
        }
        private bool GetMouseCanceledState(PMouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Released);
        }

        private bool GetMouseState(PMouseButton mouseButton, ButtonState desiredState)
        {
            ButtonState previousState, currentState;

            switch (mouseButton)
            {
                case PMouseButton.Left:
                    previousState = this._inputManager.PreviousMouseState.LeftButton;
                    currentState = this._inputManager.MouseState.LeftButton;
                    break;
                case PMouseButton.Middle:
                    previousState = this._inputManager.PreviousMouseState.MiddleButton;
                    currentState = this._inputManager.MouseState.MiddleButton;
                    break;
                case PMouseButton.Right:
                    previousState = this._inputManager.PreviousMouseState.RightButton;
                    currentState = this._inputManager.MouseState.RightButton;
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
            this.callback.State = PInputCallbackState.Started;

            OnStarted?.Invoke(this.callback);

            this.started = true;
            this.canceled = false;
        }
        private void UpdatePerformed()
        {
            this.callback.State = PInputCallbackState.Performed;

            OnPerformed?.Invoke(this.callback);

            this.performed = true;
        }
        private void UpdateCanceled()
        {
            this.callback.State = PInputCallbackState.Canceled;

            OnCanceled?.Invoke(this.callback);

            this.started = false;
            this.performed = false;
            this.canceled = true;
        }

        public struct CallbackContext
        {
            public PInputCallbackState State { get; internal set; }
            public PMouseButton CapturedMouseButton { get; internal set; }
            public Keys CapturedKey { get; internal set; }
        }
    }
}
