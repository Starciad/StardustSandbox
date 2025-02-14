using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.GameContent.GUISystem.Elements
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
