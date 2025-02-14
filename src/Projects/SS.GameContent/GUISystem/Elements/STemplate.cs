using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.GUISystem.Elements
{
    internal static class STemplate
    {
        internal static SGUILabelElement CreateTitle(ISGame game)
        {
            return new(game)
            {

            };
        }
    }
}
