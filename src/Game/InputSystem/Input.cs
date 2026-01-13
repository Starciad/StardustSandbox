/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

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

