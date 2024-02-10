using Microsoft.Xna.Framework;

using PixelDust.Game.Objects;

using System.Collections.Generic;

namespace PixelDust.Game.InputSystem.Actions
{
    public sealed class PInputActionMap(bool active) : PGameObject
    {
        public bool Active => this._active;

        private readonly PInputActionMapHandler handler;
        private readonly Dictionary<string, PInputAction> _actions = [];

        private bool _active = active;

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (PInputAction action in this._actions.Values)
            {
                action.Update(gameTime);
            }
        }

        public PInputAction AddAction(string name, PInputAction value)
        {
            if (this._actions.TryAdd(name, value))
            {
                value.SetActionMap(this);
                return value;
            }

            return default;
        }

        public void SetActive(bool value)
        {
            this._active = value;
        }
    }
}
