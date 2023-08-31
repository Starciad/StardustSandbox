using System.Collections.Generic;

namespace PixelDust.Core.Input
{
    public sealed class PInputActionMap
    {
        public bool Active => _active;

        private readonly PInputActionMapHandler handler;

        private bool _active;

        private readonly Dictionary<string, PKey> _keys = new();

        public PInputActionMap(bool active)
        {
            _active = active;
        }

        internal void Update()
        {
            foreach (PKey key in _keys.Values)
                key.Update();
        }

        public PKey AddKeyAction(string name, PKey value)
        {
            if (_keys.TryAdd(name, value))
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
