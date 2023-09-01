using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace PixelDust.Core.Elements
{
    /// <summary>
    /// Static management class responsible for managing all relevant information related to elements present and loaded in the project.
    /// </summary>
    public static class PElementsHandler
    {
        private static readonly Dictionary<uint, PElement> _elements = new();

        /// <summary>
        /// Loads, instantiates, and registers all elements that have the <see cref="PElementRegisterAttribute"/> attribute in the main assembly that the <see cref="PGame"/> class has been implemented in.
        /// </summary>
        internal static void Initialize()
        {
            foreach (Type type in PEngine.Instance.Assembly.GetTypes())
            {
                PElementRegisterAttribute register = type.GetCustomAttribute<PElementRegisterAttribute>();
                if (register == null)
                    continue;

                PElement tempElement = (PElement)Activator.CreateInstance(type);
                tempElement.Build();
                tempElement.Id = register.Id;

                if (_elements.ContainsKey(register.Id))
                {
                    throw new ArgumentException($"The id {register.Id} has already been registered in the dictionary and belongs to element {_elements[register.Id].Name}.");
                }

                _elements.Add(tempElement.Id, tempElement);
            }
        }

        /// <summary>
        /// Gets the instance of a specific element registered internally through its id and the generic specification of which element will be returned.
        /// </summary>
        /// <typeparam name="T">The type of element to be returned.</typeparam>
        /// <param name="id">The identifier number of the fetched element.</param>
        /// <returns>The requested element.</returns>
        public static T GetElementById<T>(uint id) where T : PElement
        {
            return (T)GetElementById(id);
        }

        /// <summary>
        /// Gets the instance of an element based on its id.
        /// </summary>
        /// <param name="id">The id of the fetched element.</param>
        /// <returns>The requested element.</returns>
        public static PElement GetElementById(uint id)
        {
            if (_elements.TryGetValue(id, out PElement value))
                return value;

            return default;
        }

        /// <summary>
        /// Gets the id of a specific element based on its generic type.
        /// </summary>
        /// <typeparam name="T">The generic type of element being searched for.</typeparam>
        /// <returns>The id number of the element.</returns>
        public static int GetIdOfElementType<T>() where T : PElement
        {
            return GetIdOfElementType(typeof(T));
        }

        /// <summary>
        /// Gets the id of a specific element based on its type.
        /// </summary>
        /// <param name="type">The type of element being searched for.</param>
        /// <returns>The id number of the element.</returns>
        public static int GetIdOfElementType(Type type)
        {
            return _elements.Values.FirstOrDefault(x => x.GetType() == type).Id;
        }

        /// <summary>
        /// Gets the instance of a registered element based on its generic type.
        /// </summary>
        /// <typeparam name="T">The generic type of instance being searched for.</typeparam>
        /// <returns>The instance of the requested element.</returns>
        public static T GetElementByType<T>() where T : PElement
        {
            return (T)GetElementByType(typeof(T));
        }

        /// <summary>
        /// Gets the instance of a registered element based on its type.
        /// </summary>
        /// <param name="type">The type of instance being searched for.</param>
        /// <returns>The instance of the requested element.</returns>
        public static PElement GetElementByType(Type type)
        {
            return _elements.Values.FirstOrDefault(x => x.GetType() == type);
        }
    }
}
