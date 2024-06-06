using StardustSandbox.Game.Managers;

namespace StardustSandbox.Game.GUI.Events
{
    public sealed partial class SGUIEvents(SInputManager inputManager)
    {
        private readonly SInputManager _inputManager = inputManager;
    }
}
