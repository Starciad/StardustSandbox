using PixelDust.Game.Elements;
using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Objects;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace PixelDust.Game.Databases
{
    public sealed class PElementDatabase : PGameObject
    {
        private readonly List<Type> _elementTypes = [];
        private PElement[] _elements = [];

        protected override void OnAwake()
        {
            this._elements = new PElement[this._elementTypes.Count];

            for (int i = 0; i < this._elements.Length; i++)
            {
                Type type = this._elementTypes[i];

                PElementRegisterAttribute register = type.GetCustomAttribute<PElementRegisterAttribute>();
                if (register == null)
                {
                    return;
                }

                PElement tempElement = (PElement)Activator.CreateInstance(type);
                tempElement.Initialize(this.Game);
                tempElement.Id = register.Id;

                this._elements[tempElement.Id] = tempElement;
            }
        }

        public void RegisterElement<T>() where T : PElement
        {
            RegisterElement(typeof(T));
        }

        public void RegisterElement(Type type)
        {
            this._elementTypes.Add(type);
        }

        public T GetElementById<T>(uint id) where T : PElement
        {
            return (T)GetElementById(id);
        }

        public PElement GetElementById(uint id)
        {
            return this._elements[id];
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
            return Array.Find(this._elements, x => x.GetType() == type);
        }
    }
}
