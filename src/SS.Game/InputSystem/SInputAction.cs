using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Game.Enums.InputSystem;
using StardustSandbox.Game.Managers;
using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.InputSystem
{
    public sealed class SInputAction : SGameObject
    {
        public SInputActionMap ActionMap => this._actionMap;

        private SInputActionMap _actionMap;
        private readonly SInputManager _inputManager;

        private readonly Keys[] _keys;
        private readonly SMouseButton[] _mouseButtons;

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

        public SInputAction(SGame gameInstance, SInputManager inputManager, params Keys[] keys) : base(gameInstance)
        {
            this._inputManager = inputManager;
            this._keys = keys;
            this._mouseButtons = [];
        }

        public SInputAction(SGame gameInstance, SInputManager inputManager, params SMouseButton[] mouseButtons) : base(gameInstance)
        {
            this._inputManager = inputManager;
            this._keys = [];
            this._mouseButtons = mouseButtons;
        }

        public SInputAction(SGame gameInstance, SInputManager inputManager, Keys[] keys, SMouseButton[] mouseButtons) : base(gameInstance)
        {
            this._inputManager = inputManager;
            this._keys = keys;
            this._mouseButtons = mouseButtons;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.callback = new();

            foreach (Keys key in this._keys)
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

            foreach (SMouseButton mouseButton in this._mouseButtons)
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

        internal void SetActionMap(SInputActionMap map)
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
        private bool GetMouseStartedState(SMouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Pressed);
        }
        private bool GetMousePerformedState(SMouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Pressed);
        }
        private bool GetMouseCanceledState(SMouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Released);
        }

        private bool GetMouseState(SMouseButton mouseButton, ButtonState desiredState)
        {
            ButtonState previousState, currentState;

            switch (mouseButton)
            {
                case SMouseButton.Left:
                    previousState = this._inputManager.PreviousMouseState.LeftButton;
                    currentState = this._inputManager.MouseState.LeftButton;
                    break;
                case SMouseButton.Middle:
                    previousState = this._inputManager.PreviousMouseState.MiddleButton;
                    currentState = this._inputManager.MouseState.MiddleButton;
                    break;
                case SMouseButton.Right:
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
            this.callback.State = SInputCallbackState.Started;

            OnStarted?.Invoke(this.callback);

            this.started = true;
            this.canceled = false;
        }
        private void UpdatePerformed()
        {
            this.callback.State = SInputCallbackState.Performed;

            OnPerformed?.Invoke(this.callback);

            this.performed = true;
        }
        private void UpdateCanceled()
        {
            this.callback.State = SInputCallbackState.Canceled;

            OnCanceled?.Invoke(this.callback);

            this.started = false;
            this.performed = false;
            this.canceled = true;
        }

        public struct CallbackContext
        {
            public SInputCallbackState State { get; internal set; }
            public SMouseButton CapturedMouseButton { get; internal set; }
            public Keys CapturedKey { get; internal set; }
        }
    }
}
