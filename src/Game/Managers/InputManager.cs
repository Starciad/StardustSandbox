using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StardustSandbox.Managers
{
    internal sealed class InputManager
    {
        internal MouseState MouseState => this.mouseState;
        internal MouseState PreviousMouseState => this.previousMouseState;
        internal KeyboardState KeyboardState => this.keyboardState;
        internal KeyboardState PreviousKeyboardState => this.previousKeyboardState;

        private MouseState mouseState;
        private MouseState previousMouseState;
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;

        private VideoManager videoManager;

        internal void Initialize(VideoManager videoManager)
        {
            this.videoManager = videoManager;
        }

        internal void Update()
        {
            this.previousMouseState = this.mouseState;
            this.previousKeyboardState = this.keyboardState;

            this.mouseState = Mouse.GetState();
            this.keyboardState = Keyboard.GetState();
        }

        internal Vector2 GetScaledMousePosition()
        {
            return GameRenderer.CalculateScaledMousePosition(this.mouseState.Position.ToVector2(), this.videoManager);
        }

        internal Vector2 GetScaledPreviousMousePosition()
        {
            return GameRenderer.CalculateScaledMousePosition(this.previousMouseState.Position.ToVector2(), this.videoManager);
        }

        internal int GetDeltaScrollWheel()
        {
            return this.previousMouseState.ScrollWheelValue - this.mouseState.ScrollWheelValue;
        }
    }
}
