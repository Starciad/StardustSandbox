using Microsoft.Xna.Framework;

namespace StardustSandbox.UISystem.Information
{
    internal sealed class ColorButtonInfo(string name, Color color)
    {
        internal string Name => name;
        internal Color Color => color;
    }
}
