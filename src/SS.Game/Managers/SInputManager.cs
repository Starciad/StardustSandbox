﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.Managers
{
    public sealed class SInputManager(SGame gameInstance) : SGameObject(gameInstance)
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
