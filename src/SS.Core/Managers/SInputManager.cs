using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.Managers
{
    public sealed class SInputManager(ISGame gameInstance) : SGameObject(gameInstance)
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

        public int GetDeltaScrollWheel()
        {
            return this._previousMouseState.ScrollWheelValue - this._mouseState.ScrollWheelValue;
        }
    }
}
