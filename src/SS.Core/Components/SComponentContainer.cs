using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.System;
using StardustSandbox.Core.Objects;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Components
{
    public sealed class SComponentContainer(ISGame gameInstance) : SGameObject(gameInstance), ISResettable
    {
        public SComponent[] Components => [.. this.components.Values];

        private readonly Dictionary<Type, SComponent> components = [];

        public override void Initialize()
        {
            foreach (SComponent component in this.Components)
            {
                component.Initialize();
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (SComponent component in this.Components)
            {
                component.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (SComponent component in this.Components)
            {
                component.Draw(gameTime, spriteBatch);
            }
        }

        public T AddComponent<T>(T value) where T : SComponent
        {
            this.components.Add(value.GetType(), value);
            return value;
        }

        public void RemoveComponent<T>() where T : SComponent
        {
            RemoveComponent(typeof(T));
        }

        public void RemoveComponent(Type type)
        {
            _ = this.components.Remove(type);
        }

        public T GetComponent<T>() where T : SComponent
        {
            return (T)GetComponent(typeof(T));
        }

        public SComponent GetComponent(Type componentType)
        {
            return this.components[componentType];
        }

        public bool ContainsComponent<T>() where T : SComponent
        {
            return ContainsComponent(typeof(T));
        }

        public bool ContainsComponent(Type componentType)
        {
            return this.components.ContainsKey(componentType);
        }

        public void Reset()
        {
            foreach (SComponent component in this.Components)
            {
                component.Reset();
            }
        }
    }
}
