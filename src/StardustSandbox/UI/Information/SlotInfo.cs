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

        internal SlotInfo(Image background, Image icon)
        {
            this.background = background;
            this.icon = icon;
            this.label = null;
        }

        internal SlotInfo(Image background, Image icon, Label label)
        {
            this.background = background;
            this.icon = icon;
            this.label = label;
        }
    }
}
