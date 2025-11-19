using Microsoft.Xna.Framework;

namespace StardustSandbox.UISystem.Utilities
{
    internal sealed class UIColorButton(string name, Color color)
    {
        internal string Name => name;
        internal Color Color => color;
    }
}
