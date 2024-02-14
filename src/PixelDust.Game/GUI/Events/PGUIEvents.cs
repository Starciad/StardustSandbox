using PixelDust.Game.Managers;

namespace PixelDust.Game.GUI.Events
{
    public sealed partial class PGUIEvents(PInputManager inputManager)
    {
        private readonly PInputManager _inputManager = inputManager;
    }
}
