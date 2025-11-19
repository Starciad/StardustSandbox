using Microsoft.Xna.Framework;

using StardustSandbox.Managers;

namespace StardustSandbox.InputSystem.GameInput.Handlers
{
    internal sealed class CameraHandler(CameraManager cameraManager)
    {
        private readonly CameraManager cameraManager = cameraManager;

        internal void MoveCamera(Vector2 direction)
        {
            this.cameraManager.Move(direction);
        }
    }
}
