using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

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
            return CalculateScaledMousePosition(this.mouseState, this.videoManager);
        }

        internal Vector2 GetScaledPreviousMousePosition()
        {
            return CalculateScaledMousePosition(this.previousMouseState, this.videoManager);
        }

        internal int GetDeltaScrollWheel()
        {
            return this.previousMouseState.ScrollWheelValue - this.mouseState.ScrollWheelValue;
        }

        private static Vector2 CalculateScaledMousePosition(MouseState mouseState, VideoManager videoManager)
        {
            // Gets the adjusted rectangle of the render target on the screen
            Rectangle adjustedScreen = videoManager.AdjustRenderTargetOnScreen(videoManager.ScreenRenderTarget);

            // Calculates the scale used for the adjustment
            float scale = (float)adjustedScreen.Width / videoManager.ScreenRenderTarget.Width;

            // Adjusts the mouse position to the render target space
            float mouseX = (mouseState.X - adjustedScreen.X) / scale;
            float mouseY = (mouseState.Y - adjustedScreen.Y) / scale;

            // Ensures the value does not exceed the render target bounds
            mouseX = Math.Clamp(mouseX, 0, videoManager.ScreenRenderTarget.Width - 1);
            mouseY = Math.Clamp(mouseY, 0, videoManager.ScreenRenderTarget.Height - 1);

            return new Vector2(mouseX, mouseY);
        }
    }
}
