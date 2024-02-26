using StardustSandbox.Game.Collections;
using StardustSandbox.Game.GUI.Elements;
using StardustSandbox.Game.Interfaces.General;

using System;

namespace StardustSandbox.Game.GUI
{
    public sealed class SGUILayoutPool
    {
        private readonly SObjectPool objectPool = new();

        public T GetElement<T>(SGame game) where T : SGUIElement
        {
            T element;

            if (this.objectPool.TryGet(out ISPoolableObject poolableObject))
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

        public void AddElement<T>(T value) where T : SGUIElement
        {
            this.objectPool.Add(value);
        }
    }
}