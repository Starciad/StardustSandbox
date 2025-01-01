using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Background;
using StardustSandbox.Core.Background.Handlers;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Background.Handlers;
using StardustSandbox.Core.Interfaces.Managers;

namespace StardustSandbox.Core.Managers
{
    internal sealed class SBackgroundManager(ISGame gameInstance) : SManager(gameInstance), ISBackgroundManager
    {
        public Color SolidColor { get; set; } = new(64, 116, 155);
        public ISSkyHandler SkyHandler => this.skyHandler;
        public ISCelestialBodyHandler CelestialBodyHandler => this.celestialBodyHandler;
        public ISCloudHandler CloudHandler => this.cloudHandler;

        private SBackground selectedBackground;
        private SSkyHandler skyHandler;
        private SCelestialBodyHandler celestialBodyHandler;
        private SCloudHandler cloudHandler;

        public override void Initialize()
        {
            this.skyHandler = new(this.SGameInstance);
            this.celestialBodyHandler = new(this.SGameInstance);
            this.cloudHandler = new(this.SGameInstance);

            this.skyHandler.Initialize();
            this.celestialBodyHandler.Initialize();
            this.cloudHandler.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.skyHandler.Update(gameTime);
            this.celestialBodyHandler.Update(gameTime);
            this.cloudHandler.Update(gameTime);

            this.selectedBackground.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.skyHandler.Draw(gameTime, spriteBatch);
            this.celestialBodyHandler.Draw(gameTime, spriteBatch);
            this.cloudHandler.Draw(gameTime, spriteBatch);

            this.selectedBackground.Draw(gameTime, spriteBatch);
        }

        public void Reset()
        {
            this.cloudHandler.Reset();
        }

        public void SetBackground(SBackground background)
        {
            this.selectedBackground = background;
        }
    }
}
