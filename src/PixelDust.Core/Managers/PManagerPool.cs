using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace PixelDust.Core.Managers
{
    public static class PManagerPool
    {
        private static readonly Dictionary<Type, PManager> registeredManager = new();

        internal static void Initialize()
        {
            foreach (Type type in PEngine.Instance.Assembly.GetTypes().Where(x => x.GetCustomAttribute<PManagerRegisterAttribute>() != null))
            {
                PManager temp = (PManager)Activator.CreateInstance(type);
                temp.Build();
                registeredManager.Add(type, temp);
            }

            registeredManager.Values.ToList().ForEach(x => x.Initializer());
        }

        internal static void Update()
        {
            registeredManager.Values.ToList().ForEach(x => x.Update());
        }

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

        public static bool TryFindByType(Type type, out PManager result)
        {
            result = registeredManager[type];
            return result != null;
        }
    }
}
