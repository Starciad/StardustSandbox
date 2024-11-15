using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.InputSystem
{
    public sealed class SInputActionMapHandler(ISGame gameInstance) : SGameObject(gameInstance)
    {
        private readonly Dictionary<string, SInputActionMap> _maps = [];

        public override void Update(GameTime gameTime)
        {
            foreach (SInputActionMap actionMap in this._maps.Values)
            {
                if (actionMap.Active)
                {
                    actionMap.Update(gameTime);
                }
            }
        }

        public SInputActionMap AddActionMap(string name, bool active)
        {
            SInputActionMap map = new(this.SGameInstance, this, active);

            return this._maps.TryAdd(name, map) ? map : default;
        }
    }
}
