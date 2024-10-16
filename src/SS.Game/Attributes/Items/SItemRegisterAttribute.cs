using StardustSandbox.Game.Items;

using System;
using System.Diagnostics.CodeAnalysis;

namespace StardustSandbox.Game.Attributes.Items
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SItemRegisterAttribute : Attribute
    {
        public SItem Item => this.item;

        private readonly SItem item;

        public SItemRegisterAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type itemType)
        {
            if (!typeof(SItem).IsAssignableFrom(itemType))
            {
                throw new ArgumentException($"The type {itemType} does not inherit from {nameof(SItem)}.");
            }

            this.item = (SItem)Activator.CreateInstance(itemType);
        }
    }
}
