using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Managers;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.Managers
{
    internal sealed class SGameManager(ISGame gameInstance) : SGameObject(gameInstance), ISGameManager
    {
        public SGameState GameState => this.gameState;

        private readonly SGameState gameState = new();

        private readonly ISCameraManager cameraManager = gameInstance.CameraManager;
        private readonly ISWorld world = gameInstance.World;

        public override void Update(GameTime gameTime)
        {
            ClampCameraInTheWorld();
        }

        public void StartGame()
        {
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.HUD_IDENTIFIER);

            this.SGameInstance.AmbientManager.BackgroundHandler.SetBackground(this.SGameInstance.BackgroundDatabase.GetBackgroundById("ocean_1"));

            this.world.StartNew(SWorldConstants.WORLD_SIZES_TEMPLATE[2]);

            this.SGameInstance.CameraManager.Position = new(0f, -(this.world.Infos.Size.Height * SWorldConstants.GRID_SCALE));
            this.SGameInstance.GameInputController.Activate();

            this.SGameInstance.AmbientManager.CloudHandler.IsActive = true;
        }

        public void Reset()
        {
            return;
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
