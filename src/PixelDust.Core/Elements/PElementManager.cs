using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace PixelDust.Core.Elements
{
    public static class PElementManager
    {
        private static readonly Dictionary<uint, PElement> registeredElements = new();

        internal static void Load()
        {
            byte id = 1;
            foreach (Type type in PEngine.Instance.Assembly.GetTypes())
            {
                if (type.GetCustomAttribute<PElementRegisterAttribute>() == null)
                    continue;

                PElement tempElement = (PElement)Activator.CreateInstance(type);
                tempElement.Build();
                tempElement.Id = id;

                registeredElements.Add(tempElement.Id, tempElement);
                id++;
            }
        }

        public static T GetElementById<T>(uint id) where T : PElement
        {
            if (registeredElements.TryGetValue(id, out PElement value))
            {
                return (T)value;
            }

            return null;
        }

        public static T GetElementByType<T>() where T : PElement
        {
            return (T)registeredElements.Values.FirstOrDefault(x => x.GetType() == typeof(T));
        }

        public static uint GetIdOfElement<T>() where T : PElement
        {
            return registeredElements.Values.FirstOrDefault(x => x.GetType() == typeof(T)).Id;
        }
    }
}
