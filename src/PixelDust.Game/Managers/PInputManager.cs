using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PixelDust.Game.Objects;

namespace PixelDust.Game.Managers
{
    public sealed class PInputManager : PGameObject
    {
        public MouseState MouseState => _mouseState;
        public MouseState PreviousMouseState => _previousMouseState;
        public KeyboardState KeyboardState => _keyboardState;
        public KeyboardState PreviousKeyboardState => _previousKeyboardState;

        private MouseState _mouseState;
        private MouseState _previousMouseState;
        private KeyboardState _keyboardState;
        private KeyboardState _previousKeyboardState;

        protected override void OnUpdate(GameTime gameTime)
        {
            _previousMouseState = _mouseState;
            _previousKeyboardState = _keyboardState;

            _mouseState = Mouse.GetState();
            _keyboardState = Keyboard.GetState();
        }

        public int GetDeltaScrollWheel()
        {
            return _previousMouseState.ScrollWheelValue - _mouseState.ScrollWheelValue;
        }
    }
}
