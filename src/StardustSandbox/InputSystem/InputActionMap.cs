using System.Collections.Generic;

namespace StardustSandbox.InputSystem
{
    internal sealed class InputActionMap(InputActionMapHandler handler, bool active)
    {
        internal InputActionMapHandler Handler => this.handler;
        internal bool Active => this.active;

        private readonly InputActionMapHandler handler = handler;
        private readonly Dictionary<string, InputAction> actions = [];

        private bool active = active;

        internal void Update()
        {
            foreach (InputAction action in this.actions.Values)
            {
                action.Update();
            }
        }

        internal InputAction AddAction(string name, InputAction value)
        {
            if (this.actions.TryAdd(name, value))
            {
                value.SetActionMap(this);
                return value;
            }

            return default;
        }

        internal void SetActive(bool value)
        {
            this.active = value;
        }
    }
}
