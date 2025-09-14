using Microsoft.Xna.Framework;

namespace StardustSandbox.Core.GUISystem.Common.Helpers.Interactive
{
    internal sealed class SColorButton(string name, Color color)
    {
        internal string Name => name;
        internal Color Color => color;
    }
}
