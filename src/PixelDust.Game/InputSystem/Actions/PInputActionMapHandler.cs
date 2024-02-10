using Microsoft.Xna.Framework;

using PixelDust.Game.Objects;

using System.Collections.Generic;

namespace PixelDust.Game.InputSystem.Actions
{
    public sealed class PInputActionMapHandler : PGameObject
    {
        private readonly Dictionary<string, PInputActionMap> _maps = [];

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (PInputActionMap actionMap in this._maps.Values)
            {
                if (actionMap.Active)
                {
                    actionMap.Update(gameTime);
                }
            }
        }

        public PInputActionMap AddActionMap(string name, PInputActionMap map)
        {
            return this._maps.TryAdd(name, map) ? map : default;
        }
    }
}
