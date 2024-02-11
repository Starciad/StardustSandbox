using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PixelDust.Game.Objects;

namespace PixelDust.Game.Managers
{
    public sealed class PInputManager : PGameObject
    {
        public MouseState MouseState => this._mouseState;
        public MouseState PreviousMouseState => this._previousMouseState;
        public KeyboardState KeyboardState => this._keyboardState;
        public KeyboardState PreviousKeyboardState => this._previousKeyboardState;

        private MouseState _mouseState;
        private MouseState _previousMouseState;
        private KeyboardState _keyboardState;
        private KeyboardState _previousKeyboardState;

        protected override void OnUpdate(GameTime gameTime)
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
