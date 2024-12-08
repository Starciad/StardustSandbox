using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.InputSystem
{
    public sealed class SInputActionMap(ISGame gameInstance, SInputActionMapHandler handler, bool active) : SGameObject(gameInstance)
    {
        public SInputActionMapHandler Handler => this._handler;
        public bool Active => this._active;

        private readonly SInputActionMapHandler _handler = handler;
        private readonly Dictionary<string, SInputAction> _actions = [];

        private bool _active = active;

        public override void Update(GameTime gameTime)
        {
            foreach (SInputAction action in this._actions.Values)
            {
                action.Update(gameTime);
            }
        }

        public SInputAction AddAction(string name, SInputAction value)
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
