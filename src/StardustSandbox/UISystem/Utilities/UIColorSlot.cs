using StardustSandbox.UISystem.Elements.Graphics;

namespace StardustSandbox.UISystem.Utilities
{
    internal sealed class UIColorSlot
    {
        internal ImageUIElement BackgroundElement => this.backgroundElement;
        internal ImageUIElement BorderElement => this.borderElement;

        private readonly ImageUIElement backgroundElement;
        private readonly ImageUIElement borderElement;

        internal UIColorSlot(ImageUIElement backgroundElement, ImageUIElement borderElement)
        {
            this.backgroundElement = backgroundElement;
            this.borderElement = borderElement;
        }
    }
}
