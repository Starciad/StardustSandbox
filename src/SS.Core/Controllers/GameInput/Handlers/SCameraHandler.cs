using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.Managers;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers
{
    internal sealed class SCameraHandler(ISCameraManager cameraManager)
    {
        private readonly ISCameraManager cameraManager = cameraManager;

        public void MoveCamera(Vector2 direction)
        {
            this.cameraManager.Move(direction);
        }
    }
}
