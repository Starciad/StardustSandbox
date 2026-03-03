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
        internal static MouseState CurrentMouseState { get; private set; }
        internal static MouseState PreviousMouseState { get; private set; }
        internal static KeyboardState CurrentKeyboardState { get; private set; }
        internal static KeyboardState PreviousKeyboardState { get; private set; }
        internal static GamePadState CurrentGamePadState { get; private set; }
        internal static GamePadState PreviousGamePadState { get; private set; }

        internal static void Update()
        {
            PreviousMouseState = CurrentMouseState;
            PreviousKeyboardState = CurrentKeyboardState;
            PreviousGamePadState = CurrentGamePadState;

            CurrentMouseState = Mouse.GetState();
            CurrentKeyboardState = Keyboard.GetState();
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
        }

        internal static int GetDeltaScrollWheel()
        {
            return PreviousMouseState.ScrollWheelValue - CurrentMouseState.ScrollWheelValue;
        }

        internal static Vector2 GetCurrentMousePosition()
        {
            return new(CurrentMouseState.X, CurrentMouseState.Y);
        }

        internal static Vector2 GetPreviousMousePosition()
        {
            return new(PreviousMouseState.X, PreviousMouseState.Y);
        }
    }
}
