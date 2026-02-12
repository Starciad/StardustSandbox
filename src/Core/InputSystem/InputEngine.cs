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

namespace StardustSandbox.Core.InputSystem
{
    internal static class InputEngine
    {
        internal static MouseState MouseState => mouseState;
        internal static MouseState PreviousMouseState => previousMouseState;
        internal static KeyboardState KeyboardState => keyboardState;
        internal static KeyboardState PreviousKeyboardState => previousKeyboardState;

        private static MouseState mouseState;
        private static MouseState previousMouseState;
        private static KeyboardState keyboardState;
        private static KeyboardState previousKeyboardState;

        internal static void Update()
        {
            previousMouseState = mouseState;
            previousKeyboardState = keyboardState;

            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
        }

        internal static int GetDeltaScrollWheel()
        {
            return previousMouseState.ScrollWheelValue - mouseState.ScrollWheelValue;
        }

        internal static Vector2 GetMousePosition()
        {
            return new(mouseState.X, mouseState.Y);
        }

        internal static Vector2 GetPreviousMousePosition()
        {
            return new(previousMouseState.X, previousMouseState.Y);
        }
    }
}
