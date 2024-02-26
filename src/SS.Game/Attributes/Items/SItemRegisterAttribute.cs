using StardustSandbox.Game.Items;

using System;

namespace StardustSandbox.Game.Attributes.Items
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SItemRegisterAttribute : Attribute
    {
        public SItem Item => this.item;

        private readonly SItem item;

        public SItemRegisterAttribute(Type itemType)
        {
            if (!typeof(SItem).IsAssignableFrom(itemType))
            {
                throw new ArgumentException($"The type {itemType} does not inherit from {nameof(SItem)}.");
            }

            this.item = (SItem)Activator.CreateInstance(itemType);
        }
    }
}
