using Microsoft.Xna.Framework;

using PixelDust.Game.Objects;

using System.Collections.Generic;

namespace PixelDust.Game.InputSystem
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

        public PInputActionMap AddActionMap(string name, bool active)
        {
            PInputActionMap map = new(this, active);

            return this._maps.TryAdd(name, map) ? map : default;
        }
    }
}
