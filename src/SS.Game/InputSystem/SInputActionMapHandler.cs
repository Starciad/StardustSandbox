using Microsoft.Xna.Framework;

using StardustSandbox.Game.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Game.InputSystem
{
    public sealed class SInputActionMapHandler : SGameObject
    {
        private readonly Dictionary<string, SInputActionMap> _maps = [];

        public SInputActionMapHandler(SGame gameInstance) : base(gameInstance)
        {

        }

        protected override void OnUpdate(GameTime gameTime)
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
