using StardustSandbox.UI.Elements;

namespace StardustSandbox.UI.Information
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
    }
}
