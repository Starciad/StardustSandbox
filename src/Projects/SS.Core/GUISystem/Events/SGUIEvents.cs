using StardustSandbox.Core.Interfaces.Managers;

namespace StardustSandbox.Core.GUISystem.Events
{
    public sealed partial class SGUIEvents(ISInputManager inputManager)
    {
        private readonly ISInputManager _inputManager = inputManager;
    }
}
