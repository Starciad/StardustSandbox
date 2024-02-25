using System;

namespace PixelDust.Game.Items.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PItemRegisterAttribute : Attribute
    {
        public PItem Item => this.item;

        private readonly PItem item;

        public PItemRegisterAttribute(Type itemType)
        {
            if (!typeof(PItem).IsAssignableFrom(itemType))
            {
                throw new ArgumentException($"The type {itemType} does not inherit from {nameof(PItem)}.");
            }

            this.item = (PItem)Activator.CreateInstance(itemType);
        }
    }
}
