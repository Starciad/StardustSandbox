using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed class SGUIDatabase(ISGame gameInstance) : SGameObject(gameInstance), ISGUIDatabase
    {
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

        public SGUISystem GetGUISystemById(string identifier)
        {
            return this._registeredGUIs[identifier];
        }
    }
}
