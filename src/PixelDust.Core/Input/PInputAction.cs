using Microsoft.Xna.Framework.Input;

using PixelDust.Core.Engine;

using System;

namespace PixelDust.Core.Input
{
    public sealed class PInputAction
    {
        public PInputActionMap ActionMap => _actionMap;

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
            _actionMap = map;
        }

        internal void Update()
        {
            callback = new();

            foreach (Keys key in keys)
            {
                // Started
                if (GetKeyboardStartedState(key))
                {
                    callback.CapturedKey = key;
                    UpdateStarting();
                }

                // Performed
                if (GetKeyboardPerformedState(key))
                {
                    callback.CapturedKey = key;
                    UpdatePerformed();
                }

                // Canceled
                if (GetKeyboardCanceledState(key))
                {
                    callback.CapturedKey = key;
                    UpdateCanceled();
                }
            }
            foreach (PMouseButton mouseButton in mouseButtons)
            {
                // Started
                if (GetMouseStartedState(mouseButton) &&
                    !started && !performed && canceled)
                {
                    callback.CapturedMouseButton = mouseButton;
                    UpdateStarting();
                }

                // Performed
                if (GetMousePerformedState(mouseButton) &&
                    started && !canceled)
                {
                    callback.CapturedMouseButton = mouseButton;
                    UpdatePerformed();
                }

                // Canceled
                if (GetMouseCanceledState(mouseButton)&&
                    started && performed && !canceled)
                {
                    callback.CapturedMouseButton = mouseButton;
                    UpdateCanceled();
                }
            }
        }

        // Keyboard State
        private bool GetKeyboardStartedState(Keys key)
        {
            return !PInput.PreviousKeyboard.IsKeyDown(key) &&
                    PInput.Keyboard.IsKeyDown(key) &&
                   !started && !performed && canceled;
        }
        private bool GetKeyboardPerformedState(Keys key)
        {
            return PInput.PreviousKeyboard.IsKeyDown(key) &&
                   PInput.Keyboard.IsKeyDown(key) &&
                   started && !canceled;
        }
        private bool GetKeyboardCanceledState(Keys key)
        {
            return PInput.PreviousKeyboard.IsKeyDown(key) &&
                  !PInput.Keyboard.IsKeyDown(key) &&
                   started && performed && !canceled;
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
                    previousState = PInput.PreviousMouse.LeftButton;
                    currentState = PInput.Mouse.LeftButton;
                    break;
                case PMouseButton.Middle:
                    previousState = PInput.PreviousMouse.MiddleButton;
                    currentState = PInput.Mouse.MiddleButton;
                    break;
                case PMouseButton.Right:
                    previousState = PInput.PreviousMouse.RightButton;
                    currentState = PInput.Mouse.RightButton;
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
            callback.State = PInputCallbackState.Started;
            
            OnStarted?.Invoke(callback);

            started = true;
            canceled = false;
        }
        private void UpdatePerformed()
        {
            callback.State = PInputCallbackState.Performed;

            OnPerformed?.Invoke(callback);

            performed = true;
        }
        private void UpdateCanceled()
        {
            callback.State = PInputCallbackState.Canceled;

            OnCanceled?.Invoke(callback);

            started = false;
            performed = false;
            canceled = true;
        }

        public struct CallbackContext
        {
            public PInputCallbackState State { get; internal set; }
            public PMouseButton CapturedMouseButton { get; internal set; }
            public Keys CapturedKey { get; internal set; }
        }
    }
}
