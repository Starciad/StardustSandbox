using StardustSandbox.UI.Elements;

namespace StardustSandbox.UI.Information
{
    internal sealed class ColorSlotInfo
    {
        internal Image Background => this.background;
        internal Image Border => this.border;

        private readonly Image background;
        private readonly Image border;

        internal ColorSlotInfo(Image background, Image border)
        {
            this.background = background;
            this.border = border;
        }
    }
}
