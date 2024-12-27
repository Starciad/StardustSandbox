using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed class SElementDatabase(ISGame gameInstance) : SGameObject(gameInstance), ISElementDatabase
    {
        private readonly Dictionary<string, ISElement> _registeredElements = [];

        public void RegisterElement(ISElement element)
        {
            ((SElement)element).Initialize();
            this._registeredElements.Add(element.Identifier, element);
        }

        public ISElement GetElementByIdentifier(string identifier)
        {
            return this._registeredElements[identifier];
        }
    }
}
