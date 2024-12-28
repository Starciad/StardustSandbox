using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed class SGUIDatabase(ISGame gameInstance) : SGameObject(gameInstance), ISGUIDatabase
    {
        private readonly Dictionary<string, SGUISystem> registeredGUIs = [];

        public override void Initialize()
        {
            foreach (SGUISystem guiSystem in this.registeredGUIs.Values)
            {
                guiSystem.Initialize();
            }
        }

        public void RegisterGUISystem(string identifier, SGUISystem guiSystem)
        {
            this.registeredGUIs.Add(identifier, guiSystem);
        }

        public SGUISystem GetGUISystemById(string identifier)
        {
            return this.registeredGUIs[identifier];
        }
    }
}
