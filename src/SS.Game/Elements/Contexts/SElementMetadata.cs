using System;
using System.Collections.Generic;

namespace StardustSandbox.Game.Elements.Contexts
{
    public sealed class SElementMetadata
    {
        public int Count => this.infos.Count;

        private readonly Dictionary<string, object> infos = [];

        public void Copy(SElementMetadata other)
        {
            Clear();

            foreach (KeyValuePair<string, object> info in other.infos)
            {
                this.infos.Add(info.Key, info.Value);
            }
        }

        public void Add<T>(string key, T value)
        {
            if (this.infos.ContainsKey(key))
            {
                throw new ArgumentException($"The key '{key}' already exists in the metadata. Use the Update method to modify it.");
            }

            this.infos[key] = value;
        }

        public T Get<T>(string key)
        {
            return this.infos.TryGetValue(key, out object value)
                ? value is T typedValue
                    ? typedValue
                    : throw new InvalidCastException($"The value associated with key '{key}' is not of type {typeof(T)}.")
                : throw new KeyNotFoundException($"The key '{key}' was not found in the metadata.");
        }

        public void Update<T>(string key, T newValue)
        {
            if (!this.infos.ContainsKey(key))
            {
                throw new KeyNotFoundException($"The key '{key}' does not exist in the metadata. Use the Add method to create it.");
            }

            this.infos[key] = newValue;
        }

        public void Remove(string key)
        {
            if (!this.infos.Remove(key))
            {
                throw new KeyNotFoundException($"The key '{key}' was not found in the metadata.");
            }
        }

        public void Clear()
        {
            this.infos.Clear();
        }

        public bool ContainsKey(string key)
        {
            return this.infos.ContainsKey(key);
        }
    }
}
