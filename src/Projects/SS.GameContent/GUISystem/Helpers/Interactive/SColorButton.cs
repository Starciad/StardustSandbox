using Microsoft.Xna.Framework;

namespace StardustSandbox.GameContent.GUISystem.Helpers.Interactive
{
    internal sealed class SColorButton(string name, Color color)
    {
        internal string Name => name;
        internal Color Color => color;
    }
}
