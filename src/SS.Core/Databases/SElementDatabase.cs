using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Objects;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    public sealed class SElementDatabase(ISGame gameInstance) : SGameObject(gameInstance)
    {
        private readonly List<ISElement> _registeredElements = [];

        public void RegisterElement(SElement element)
        {
            element.Initialize();
            this._registeredElements.Add(element);
        }

        public T GetElementById<T>(uint id) where T : ISElement
        {
            return (T)GetElementById(id);
        }

        public ISElement GetElementById(uint id)
        {
            return this._registeredElements[(int)id];
        }

        public uint GetIdOfElementType<T>() where T : ISElement
        {
            return GetIdOfElementType(typeof(T));
        }

        public uint GetIdOfElementType(Type type)
        {
            return GetElementByType(type).Id;
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
