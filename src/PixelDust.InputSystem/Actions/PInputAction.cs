using Microsoft.Xna.Framework.Input;

using PixelDust.InputSystem.Enums;
using PixelDust.InputSystem.Handlers;

using System;

namespace PixelDust.InputSystem.Actions
{
    public sealed class PInputAction
    {
        public PInputActionMap ActionMap => this._actionMap;

        private PInputActionMap _actionMap;

        private readonly Keys[] keys = Array.Empty<Keys>();
        private readonly PMouseButton[] mouseButtons = Array.Empty<PMouseButton>();

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

        public PInputAction(params Keys[] keys)
        {
            this.keys = keys;
        }
        public PInputAction(params PMouseButton[] mouseButtons)
        {
            this.mouseButtons = mouseButtons;
        }
        public PInputAction(Keys[] keys, PMouseButton[] mouseButtons)
        {
            this.keys = keys;
            this.mouseButtons = mouseButtons;
        }

        internal void SetActionMap(PInputActionMap map)
        {
            this._actionMap = map;
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

        // Keyboard State
        private bool GetKeyboardStartedState(Keys key)
        {
            return !PInputHandler.PreviousKeyboard.IsKeyDown(key) &&
                    PInputHandler.Keyboard.IsKeyDown(key) &&
                   !this.started && !this.performed && this.canceled;
        }
        private bool GetKeyboardPerformedState(Keys key)
        {
            return PInputHandler.PreviousKeyboard.IsKeyDown(key) &&
                   PInputHandler.Keyboard.IsKeyDown(key) &&
                   this.started && !this.canceled;
        }
        private bool GetKeyboardCanceledState(Keys key)
        {
            return PInputHandler.PreviousKeyboard.IsKeyDown(key) &&
                  !PInputHandler.Keyboard.IsKeyDown(key) &&
                   this.started && this.performed && !this.canceled;
        }

        // Mouse State
        private static bool GetMouseStartedState(PMouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Pressed);
        }
        private static bool GetMousePerformedState(PMouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Pressed);
        }
        private static bool GetMouseCanceledState(PMouseButton mouseButton)
        {
            return GetMouseState(mouseButton, ButtonState.Released);
        }

        private static bool GetMouseState(PMouseButton mouseButton, ButtonState desiredState)
        {
            ButtonState previousState, currentState;

            switch (mouseButton)
            {
                case PMouseButton.Left:
                    previousState = PInputHandler.PreviousMouse.LeftButton;
                    currentState = PInputHandler.Mouse.LeftButton;
                    break;
                case PMouseButton.Middle:
                    previousState = PInputHandler.PreviousMouse.MiddleButton;
                    currentState = PInputHandler.Mouse.MiddleButton;
                    break;
                case PMouseButton.Right:
                    previousState = PInputHandler.PreviousMouse.RightButton;
                    currentState = PInputHandler.Mouse.RightButton;
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
