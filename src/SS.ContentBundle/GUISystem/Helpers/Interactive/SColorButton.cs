using Microsoft.Xna.Framework;

namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive
{
    internal sealed class SColorButton(string displayName, Color color)
    {
        internal string DisplayName => displayName;
        internal Color Color => color;
    }
}
