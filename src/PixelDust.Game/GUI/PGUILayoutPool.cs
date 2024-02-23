using PixelDust.Game.Collections;
using PixelDust.Game.GUI.Elements;

using System;

namespace PixelDust.Game.GUI
{
    public sealed class PGUILayoutPool
    {
        private readonly PObjectPool objectPool = new();

        public T GetElement<T>(PGame game) where T : PGUIElement
        {
            T element;

            if (this.objectPool.TryGet(out IPoolableObject poolableObject))
            {
                element = (T)poolableObject;
            }
            else
            {
                element = Activator.CreateInstance<T>();
                element.Reset();
            }

            element.Initialize(game);

            return element;
        }

        public void AddElement<T>(T value) where T : PGUIElement
        {
            this.objectPool.Add(value);
        }
    }
}