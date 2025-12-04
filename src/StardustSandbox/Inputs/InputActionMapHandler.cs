using System.Collections.Generic;

namespace StardustSandbox.Inputs
{
    internal sealed class InputActionMapHandler
    {
        private readonly Dictionary<string, InputActionMap> maps = [];

        internal void Update()
        {
            foreach (InputActionMap actionMap in this.maps.Values)
            {
                if (actionMap.Active)
                {
                    actionMap.Update();
                }
            }
        }

        internal InputActionMap AddActionMap(string name, bool active)
        {
            InputActionMap map = new(this, active);

            return this.maps.TryAdd(name, map) ? map : default;
        }

        internal InputActionMap GetActionMap(string name)
        {
            return this.maps[name];
        }

        internal void ActivateAll()
        {
            foreach (InputActionMap actionMap in this.maps.Values)
            {
                actionMap.SetActive(true);
            }
        }

        internal void DisableAll()
        {
            foreach (InputActionMap actionMap in this.maps.Values)
            {
                actionMap.SetActive(false);
            }
        }
    }
}
