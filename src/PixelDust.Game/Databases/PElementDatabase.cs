using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Objects;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace PixelDust.Game.Elements
{
    public sealed class PElementDatabase : PGameObject
    {
        private readonly List<Type> _elementTypes = [];
        private PElement[] _elements = [];

        protected override void OnAwake()
        {
            this._elements = new PElement[this._elementTypes.Count];

            for (int i = 0; i < _elementTypes.Count; i++)
            {
                Type type = _elementTypes[i];
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

        public void AddElement<T>() where T : PElement
        {
            AddElement(typeof(T));
        }

        public void AddElement(Type type)
        {
            this._elementTypes.Add(type);
        }

        public T GetElementById<T>(uint id) where T : PElement
        {
            return (T)GetElementById(id);
        }

        public PElement GetElementById(uint id)
        {
            return _elements[id];
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
