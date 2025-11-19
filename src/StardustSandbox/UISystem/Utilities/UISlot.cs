using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;

namespace StardustSandbox.UISystem.Utilities
{
    internal sealed class UISlot
    {
        internal ImageUIElement BackgroundElement => this.backgroundElement;
        internal ImageUIElement IconElement => this.iconElement;
        internal LabelUIElement LabelElement => this.labelElement;

        private readonly ImageUIElement backgroundElement;
        private readonly ImageUIElement iconElement;
        private readonly LabelUIElement labelElement;

        internal UISlot(ImageUIElement backgroundElement, ImageUIElement iconElement)
        {
            this.backgroundElement = backgroundElement;
            this.iconElement = iconElement;
            this.labelElement = null;
        }

        internal UISlot(ImageUIElement backgroundElement, ImageUIElement iconElement, LabelUIElement labelElement)
        {
            this.backgroundElement = backgroundElement;
            this.iconElement = iconElement;
            this.labelElement = labelElement;
        }

        internal void Show()
        {
            if (this.backgroundElement != null)
            {
                this.backgroundElement.IsVisible = true;
            }

            if (this.iconElement != null)
            {
                this.iconElement.IsVisible = true;
            }

            if (this.labelElement != null)
            {
                this.labelElement.IsVisible = true;
            }
        }

        internal void Hide()
        {
            if (this.backgroundElement != null)
            {
                this.backgroundElement.IsVisible = false;
            }

            if (this.iconElement != null)
            {
                this.iconElement.IsVisible = false;
            }

            if (this.labelElement != null)
            {
                this.labelElement.IsVisible = false;
            }
        }
    }
}
