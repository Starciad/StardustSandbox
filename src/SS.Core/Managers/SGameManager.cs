using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.Managers;
using StardustSandbox.Core.Objects;
using StardustSandbox.Core.World;

namespace StardustSandbox.Core.Managers
{
    internal sealed class SGameManager(ISGame gameInstance) : SGameObject(gameInstance), ISGameManager
    {
        public SGameState GameState => this.gameState;

        private readonly SGameState gameState = new();

        private readonly SCameraManager cameraManager = gameInstance.CameraManager;
        private readonly SWorld world = gameInstance.World;

        public override void Update(GameTime gameTime)
        {
            ClampCameraInTheWorld();
        }

        private void ClampCameraInTheWorld()
        {
            int totalWorldWidth = this.world.Infos.Size.Width * SWorldConstants.GRID_SCALE;
            int totalWorldHeight = this.world.Infos.Size.Height * SWorldConstants.GRID_SCALE;

            float visibleWidth = SScreenConstants.DEFAULT_SCREEN_WIDTH;
            float visibleHeight = SScreenConstants.DEFAULT_SCREEN_HEIGHT;

            float worldLeftLimit = 0f;
            float worldRightLimit = totalWorldWidth - visibleWidth;

            float worldBottomLimit = (totalWorldHeight - visibleHeight) * -1;
            float worldTopLimit = 0f;

            Vector2 cameraPosition = this.cameraManager.Position;

            cameraPosition.X = MathHelper.Clamp(cameraPosition.X, worldLeftLimit, worldRightLimit);
            cameraPosition.Y = MathHelper.Clamp(cameraPosition.Y, worldBottomLimit, worldTopLimit);

            this.cameraManager.Position = cameraPosition;
        }
    }
}
