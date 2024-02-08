using System.Collections.Generic;

namespace PixelDust.InputSystem.Actions
{
    public sealed class PInputActionMapHandler
    {
        private readonly Dictionary<string, PInputActionMap> _maps = new();

        public void Update()
        {
            foreach (PInputActionMap actionMap in this._maps.Values)
            {
                if (actionMap.Active)
                {
                    actionMap.Update();
                }
            }
        }

        public PInputActionMap AddActionMap(string name, PInputActionMap map)
        {
            return this._maps.TryAdd(name, map) ? map : default;
        }
    }
}
