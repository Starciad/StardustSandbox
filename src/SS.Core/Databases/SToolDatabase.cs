using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.Tools;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed class SToolDatabase(ISGame gameInstance) : SGameObject(gameInstance), ISToolDatabase
    {
        private readonly Dictionary<string, ISTool> registeredTools = [];

        public void RegisterTool(ISTool tool)
        {
            this.registeredTools.Add(tool.Identifier, tool);
        }

        public ISTool GetToolByIdentifier(string identifier)
        {
            return this.registeredTools[identifier];
        }
    }
}
