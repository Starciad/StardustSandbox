using Microsoft.Xna.Framework.Input;

namespace PixelDust.Core
{
    public static class PInput
    {
        public static MouseState MouseState => _mouseState;
        public static KeyboardState KeyboardState => _keyboardState;

        private static MouseState _mouseState;
        private static KeyboardState _keyboardState;

        internal static void Update()
        {
            _mouseState = Mouse.GetState();
            _keyboardState = Keyboard.GetState();
        }
    }
}
