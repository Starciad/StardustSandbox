using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    public sealed class SGUIDatabase(ISGame gameInstance) : SGameObject(gameInstance)
    {
        public SGUISystem[] RegisteredGUIs => [.. this._registeredGUIs.Values];

        private readonly Dictionary<string, SGUISystem> _registeredGUIs = [];

        public override void Initialize()
        {
            foreach (SGUISystem guiSystem in this._registeredGUIs.Values)
            {
                guiSystem.Initialize();
            }
        }

        public void RegisterGUISystem(string identifier, SGUISystem guiSystem)
        {
            this._registeredGUIs.Add(identifier, guiSystem);
        }

        public SGUISystem Find(string identifier)
        {
            return this._registeredGUIs[identifier];
        }
    }
}
