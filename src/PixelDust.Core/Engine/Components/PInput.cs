using Microsoft.Xna.Framework.Input;

namespace PixelDust.Core.Engine
{
    /// <summary>
    /// Static wrapper of engine input information.
    /// </summary>
    public static class PInput
    {
        /// <summary>
        /// Current state the player's mouse is in.
        /// </summary>
        public static MouseState MouseState => _mouseState;

        /// <summary>
        /// Current state the player's keyboard is in.
        /// </summary>
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
