using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Elements;
using PixelDust.Game.Objects;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace PixelDust.Game.Databases
{
    public sealed class PElementDatabase : PGameObject
    {
        private List<PElement> _registeredElements = [];

        public void Build()
        {
            this._registeredElements = [.. this._registeredElements.OrderBy(x => x.Id)];
        }

        public void RegisterElement(PElement element)
        {
            element.Initialize(this.Game);
            element.Id = element.GetType().GetCustomAttribute<PElementRegisterAttribute>().Id;

            this._registeredElements.Add(element);
        }

        public T GetElementById<T>(uint id) where T : PElement
        {
            return (T)GetElementById(id);
        }

        public PElement GetElementById(uint id)
        {
            return this._registeredElements[(int)id];
        }

        public uint GetIdOfElementType<T>() where T : PElement
        {
            return GetIdOfElementType(typeof(T));
        }

        public uint GetIdOfElementType(Type type)
        {
            return GetElementByType(type).Id;
        }

        public T GetElementByType<T>() where T : PElement
        {
            return (T)GetElementByType(typeof(T));
        }

        public PElement GetElementByType(Type type)
        {
            return this._registeredElements.Find(x => x.GetType() == type);
        }
    }
}
