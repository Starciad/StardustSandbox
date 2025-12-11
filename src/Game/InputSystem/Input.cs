using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Managers;

using System;

namespace StardustSandbox.InputSystem
{
    internal static class Input
    {
        internal static MouseState MouseState => mouseState;
        internal static MouseState PreviousMouseState => previousMouseState;
        internal static KeyboardState KeyboardState => keyboardState;
        internal static KeyboardState PreviousKeyboardState => previousKeyboardState;

        private static MouseState mouseState;
        private static MouseState previousMouseState;
        private static KeyboardState keyboardState;
        private static KeyboardState previousKeyboardState;

        private static VideoManager videoManager;

        private static bool isInitialized = false;

        internal static void Initialize(VideoManager videoManager)
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(Input)} has already been initialized.");
            }

            Input.videoManager = videoManager;

            isInitialized = true;
        }

        internal static void Update()
        {
            previousMouseState = mouseState;
            previousKeyboardState = keyboardState;

            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
        }

        internal static Vector2 GetScaledMousePosition()
        {
            return GameRenderer.CalculateScaledMousePosition(mouseState.Position.ToVector2(), videoManager);
        }

        internal static Vector2 GetScaledPreviousMousePosition()
        {
            return GameRenderer.CalculateScaledMousePosition(previousMouseState.Position.ToVector2(), videoManager);
        }

        internal static int GetDeltaScrollWheel()
        {
            return previousMouseState.ScrollWheelValue - mouseState.ScrollWheelValue;
        }
    }
}
