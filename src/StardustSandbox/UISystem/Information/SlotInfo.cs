using StardustSandbox.UISystem.Elements;

namespace StardustSandbox.UISystem.Information
{
    internal sealed class SlotInfo
    {
        internal Image Background => this.background;
        internal Image Icon => this.icon;
        internal Label Label => this.label;

        private readonly Image background;
        private readonly Image icon;
        private readonly Label label;

        internal SlotInfo(Image backgroundElement, Image iconElement)
        {
            this.background = backgroundElement;
            this.icon = iconElement;
            this.label = null;
        }

        internal SlotInfo(Image backgroundElement, Image iconElement, Label label)
        {
            this.background = backgroundElement;
            this.icon = iconElement;
            this.label = label;
        }

        internal void Show()
        {
            if (this.background != null)
            {
                this.background.CanDraw = true;
            }

            if (this.icon != null)
            {
                this.icon.CanDraw = true;
            }

            if (this.label != null)
            {
                this.label.CanDraw = true;
            }
        }

        internal void Hide()
        {
            if (this.background != null)
            {
                this.background.CanDraw = false;
            }

            if (this.icon != null)
            {
                this.icon.CanDraw = false;
            }

            if (this.label != null)
            {
                this.label.CanDraw = false;
            }
        }
    }
}
