using System;

namespace PixelDust.Game.Items.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PItemRegisterAttribute : Attribute
    {
        public PItemDetails Details => this.details;

        private readonly PItemDetails details;

        public PItemRegisterAttribute(Type itemDetailsType)
        {
            if (!typeof(PItemDetails).IsAssignableFrom(itemDetailsType))
            {
                throw new ArgumentException($"The type {itemDetailsType} does not inherit from {nameof(PItemDetails)}.");
            }

            this.details = (PItemDetails)Activator.CreateInstance(itemDetailsType);
        }
    }
}
