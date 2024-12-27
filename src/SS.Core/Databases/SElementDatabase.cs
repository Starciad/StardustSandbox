using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Objects;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed class SElementDatabase(ISGame gameInstance) : SGameObject(gameInstance), ISElementDatabase
    {
        private readonly List<ISElement> _registeredElements = [];

        public void RegisterElement(SElement element)
        {
            element.Initialize();
            this._registeredElements.Add(element);
        }

        public T GetElementById<T>(uint identifier) where T : ISElement
        {
            return (T)GetElementById(identifier);
        }

        public ISElement GetElementById(uint identifier)
        {
            return this._registeredElements[(int)identifier];
        }

        public uint GetIdOfElementType<T>() where T : ISElement
        {
            return GetIdOfElementType(typeof(T));
        }

        public uint GetIdOfElementType(Type type)
        {
            return GetElementByType(type).Identifier;
        }

        public T GetElementByType<T>() where T : ISElement
        {
            return (T)GetElementByType(typeof(T));
        }

        public ISElement GetElementByType(Type type)
        {
            return this._registeredElements.Find(x => x.GetType() == type);
        }
    }
}
