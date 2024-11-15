using StardustSandbox.Game.GUISystem;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Game.Databases
{
    public sealed class SGUIDatabase(ISGame gameInstance) : SGameObject(gameInstance)
    {
        public SGUISystem[] RegisteredGUIs => [.. this._registeredGUIs];

        private readonly List<SGUISystem> _registeredGUIs = [];

        public override void Initialize()
        {
            this._registeredGUIs.ForEach(x => x.Initialize());
        }

        public void RegisterGUISystem(SGUISystem guiSystem)
        {
            this._registeredGUIs.Add(guiSystem);
        }

        public SGUISystem Find(string name)
        {
            return this._registeredGUIs.Find(x => x.Name == name);
        }
    }
}
