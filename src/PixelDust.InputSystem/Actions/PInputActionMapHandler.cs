using System.Collections.Generic;

namespace PixelDust.InputSystem
{
    public sealed class PInputActionMapHandler
    {
        private readonly Dictionary<string, PInputActionMap> _maps = new();

        public void Update()
        {
            foreach (PInputActionMap actionMap in _maps.Values)
            {
                if (actionMap.Active)
                    actionMap.Update();
            }
        }

        public PInputActionMap AddActionMap(string name, PInputActionMap map)
        {
            if (_maps.TryAdd(name, map))
            {
                return map;
            }

            return default;
        }
    }
}
