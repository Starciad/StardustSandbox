using Microsoft.Xna.Framework;

namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive
{
    internal sealed class SColorButton(string name, Color color)
    {
        internal string Name => name;
        internal Color Color => color;
    }
}
