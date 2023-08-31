using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace PixelDust.Core.Managers
{
    /// <summary>
    /// Static class responsible for registering, instantiating and manipulating all project managers.
    /// </summary>
    public static class PManagersHandler
    {
        private static readonly Dictionary<Type, PManager> _managers = new();

        /// <summary>
        /// Registers, instantiates, configures and initializes all project managers that have the <see cref="PManagerRegisterAttribute"/> attribute.
        /// </summary>
        internal static void Initialize()
        {
            foreach (Type type in PEngine.Instance.Assembly.GetTypes().Where(x => x.GetCustomAttribute<PManagerRegisterAttribute>() != null))
            {
                PManager temp = (PManager)Activator.CreateInstance(type);
                temp.Awake();
                _managers.Add(type, temp);
            }

            _managers.Values.ToList().ForEach(x => x.Start());
        }

        /// <summary>
        /// Updates all instantiated managers.
        /// </summary>
        internal static void Update()
        {
            _managers.Values.ToList().ForEach(x => x.Update());
        }

        /// <summary>
        /// Draws all instantiated managers.
        /// </summary>
        internal static void Draw()
        {
            _managers.Values.ToList().ForEach(x => x.Draw());
        }

        /// <summary>
        /// Tries to find a project manager instance of a specified type.
        /// </summary>
        /// <typeparam name="T">The type of the project manager to find.</typeparam>
        /// <param name="result">The found project manager instance.</param>
        /// <returns>Returns true if the project manager is found, otherwise false.</returns>
        public static bool TryFindByType<T>(out T result) where T : PManager
        {
            if (TryFindByType(typeof(T), out PManager r))
            {
                result = (T)r;
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Tries to find a project manager instance of a specified type.
        /// </summary>
        /// <param name="type">The type of the project manager to find.</param>
        /// <param name="result">The found project manager instance.</param>
        /// <returns>Returns true if the project manager is found, otherwise false.</returns>
        public static bool TryFindByType(Type type, out PManager result)
        {
            result = _managers[type];
            return result != null;
        }
    }
}
