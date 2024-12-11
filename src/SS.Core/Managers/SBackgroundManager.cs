using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Backgrounds;
using StardustSandbox.Core.Controllers.Background;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.Managers;

namespace StardustSandbox.Core.Managers
{
    internal sealed class SBackgroundManager(ISGame gameInstance) : SManager(gameInstance), ISBackgroundManager
    {
        public Color SolidColor { get; set; } = new(64, 116, 155);

        private SBackground selectedBackground;
        private SCloudController cloudController;

        public override void Initialize()
        {
            this.cloudController = new(this.SGameInstance);
        }

        public override void Update(GameTime gameTime)
        {
            this.cloudController.Update(gameTime);
            this.selectedBackground.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.cloudController.Draw(gameTime, spriteBatch);
            this.selectedBackground.Draw(gameTime, spriteBatch);
        }

        public void SetBackground(SBackground background)
        {
            this.selectedBackground = background;
        }

        public void EnableClouds()
        {
            this.cloudController.Enable();
        }

        public void DisableClouds()
        {
            this.cloudController.Disable();
            this.cloudController.Clear();
        }
    }
}
