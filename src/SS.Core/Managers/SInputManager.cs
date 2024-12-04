using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.Managers
{
    public sealed class SInputManager(ISGame gameInstance) : SManager(gameInstance)
    {
        public MouseState MouseState => this._mouseState;
        public MouseState PreviousMouseState => this._previousMouseState;
        public KeyboardState KeyboardState => this._keyboardState;
        public KeyboardState PreviousKeyboardState => this._previousKeyboardState;

        private MouseState _mouseState;
        private MouseState _previousMouseState;
        private KeyboardState _keyboardState;
        private KeyboardState _previousKeyboardState;

        public override void Update(GameTime gameTime)
        {
            this._previousMouseState = this._mouseState;
            this._previousKeyboardState = this._keyboardState;

            this._mouseState = Mouse.GetState();
            this._keyboardState = Keyboard.GetState();
        }

        public Vector2 GetScaledMousePosition()
        {
            return CalculateScaledMousePosition(this._mouseState, this.SGameInstance.GraphicsManager);
        }

        public Vector2 GetScaledPreviousMousePosition()
        {
            return CalculateScaledMousePosition(this._previousMouseState, this.SGameInstance.GraphicsManager);
        }

        public int GetDeltaScrollWheel()
        {
            return this._previousMouseState.ScrollWheelValue - this._mouseState.ScrollWheelValue;
        }

        private static Vector2 CalculateScaledMousePosition(MouseState mouseState, SGraphicsManager graphicsManager)
        {
            Vector2 mousePosition = new(mouseState.X, mouseState.Y);

            mousePosition /= graphicsManager.GetScreenScaleFactor();

            return mousePosition;
        }
    }
}
