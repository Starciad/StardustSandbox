using StardustSandbox.Core.GUISystem.Common.Elements.Graphics;
using StardustSandbox.Core.GUISystem.Common.Elements.Textual;

namespace StardustSandbox.Core.GUISystem.Common.Helpers.General
{
    internal sealed class SSlot
    {
        internal SGUIImageElement BackgroundElement => this.backgroundElement;
        internal SGUIImageElement IconElement => this.iconElement;
        internal SGUILabelElement LabelElement => this.labelElement;

        private readonly SGUIImageElement backgroundElement;
        private readonly SGUIImageElement iconElement;
        private readonly SGUILabelElement labelElement;

        internal SSlot(SGUIImageElement backgroundElement, SGUIImageElement iconElement)
        {
            this.backgroundElement = backgroundElement;
            this.iconElement = iconElement;
            this.labelElement = null;
        }

        internal SSlot(SGUIImageElement backgroundElement, SGUIImageElement iconElement, SGUILabelElement labelElement)
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
