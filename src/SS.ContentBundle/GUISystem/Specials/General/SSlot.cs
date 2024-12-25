using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;

namespace StardustSandbox.ContentBundle.GUISystem.Specials.General
{
    internal sealed class SSlot
    {
        internal SGUIImageElement BackgroundElement => this.backgroundElement;
        internal SGUIImageElement IconElement => this.iconElement;
        internal SGUILabelElement LabelElement => this.labelElement;

        private readonly SGUIImageElement backgroundElement;
        private readonly SGUIImageElement iconElement;
        private readonly SGUILabelElement labelElement;

        public SSlot(SGUIImageElement backgroundElement, SGUIImageElement iconElement)
        {
            this.backgroundElement = backgroundElement;
            this.iconElement = iconElement;
            this.labelElement = null;
        }

        public SSlot(SGUIImageElement backgroundElement, SGUIImageElement iconElement, SGUILabelElement labelElement)
        {
            this.backgroundElement = backgroundElement;
            this.iconElement = iconElement;
            this.labelElement = labelElement;
        }
    }
}
