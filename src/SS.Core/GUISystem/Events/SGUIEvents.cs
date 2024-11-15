using StardustSandbox.Core.Managers;

namespace StardustSandbox.Core.GUI.Events
{
    public sealed partial class SGUIEvents(SInputManager inputManager)
    {
        private readonly SInputManager _inputManager = inputManager;
    }
}
