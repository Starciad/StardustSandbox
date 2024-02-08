using System.Collections.Generic;

namespace PixelDust.InputSystem.Actions
{
    public sealed class PInputActionMap
    {
        public bool Active => this._active;

        private bool _active;

#pragma warning disable CS0169 // O campo "PInputActionMap.handler" nunca é usado
        private readonly PInputActionMapHandler handler;
#pragma warning restore CS0169 // O campo "PInputActionMap.handler" nunca é usado

        private readonly Dictionary<string, PInputAction> _actions = new();

        public PInputActionMap(bool active)
        {
            this._active = active;
        }

        internal void Update()
        {
            foreach (PInputAction action in this._actions.Values)
            {
                action.Update();
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
