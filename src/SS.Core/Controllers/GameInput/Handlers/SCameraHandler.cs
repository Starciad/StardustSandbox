using Microsoft.Xna.Framework;

using StardustSandbox.Core.Managers;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers
{
    internal sealed class SCameraHandler(SCameraManager cameraManager)
    {
        private readonly SCameraManager cameraManager = cameraManager;

        public void MoveCamera(Vector2 direction)
        {
            this.cameraManager.Move(direction);
        }
    }
}
