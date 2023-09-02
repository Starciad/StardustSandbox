using System.Collections.Generic;

namespace PixelDust.Core.Input
{
    public sealed class PInputActionMap
    {
        public bool Active => _active;

        private bool _active;

#pragma warning disable CS0169 // O campo "PInputActionMap.handler" nunca é usado
        private readonly PInputActionMapHandler handler;
#pragma warning restore CS0169 // O campo "PInputActionMap.handler" nunca é usado

        private readonly Dictionary<string, PInputAction> _actions = new();

        public PInputActionMap(bool active)
        {
            _active = active;
        }

        internal void Update()
        {
            foreach (PInputAction action in _actions.Values)
                action.Update();
        }

        public PInputAction AddAction(string name, PInputAction value)
        {
            if (_actions.TryAdd(name, value))
            {
                value.SetActionMap(this);
                return value;
            }

            return default;
        }

        public void SetActive(bool value)
        {
            _active = value;
        }
    }
}
