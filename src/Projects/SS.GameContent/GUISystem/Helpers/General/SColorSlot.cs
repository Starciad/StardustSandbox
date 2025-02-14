using StardustSandbox.GameContent.GUISystem.Elements.Graphics;

namespace StardustSandbox.GameContent.GUISystem.Helpers.General
{
    internal sealed class SColorSlot
    {
        internal SGUIImageElement BackgroundElement => this.backgroundElement;
        internal SGUIImageElement BorderElement => this.borderElement;

        private readonly SGUIImageElement backgroundElement;
        private readonly SGUIImageElement borderElement;

        internal SColorSlot(SGUIImageElement backgroundElement, SGUIImageElement borderElement)
        {
            this.backgroundElement = backgroundElement;
            this.borderElement = borderElement;
        }
    }
}
