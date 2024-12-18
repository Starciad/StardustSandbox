using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;

namespace StardustSandbox.ContentBundle.GUISystem.Specials.General
{
    internal sealed class SSlot(SGUIImageElement backgroundElement, SGUIImageElement iconElement)
    {
        internal SGUIImageElement BackgroundElement => backgroundElement;
        internal SGUIImageElement IconElement => iconElement;
    }
}
