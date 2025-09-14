using Microsoft.Xna.Framework;

using StardustSandbox.Core.Ambient.Handlers;
using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Ambient.Handlers;
using StardustSandbox.Core.Interfaces.Managers;

namespace StardustSandbox.Core.Managers
{
    internal sealed class SAmbientManager(ISGame gameInstance) : SManager(gameInstance), ISAmbientManager
    {
        public ISTimeHandler TimeHandler => this.timeHandler;
        public ISBackgroundHandler BackgroundHandler => this.backgroundHandler;
        public ISSkyHandler SkyHandler => this.skyHandler;
        public ISCelestialBodyHandler CelestialBodyHandler => this.celestialBodyHandler;
        public ISCloudHandler CloudHandler => this.cloudHandler;

        private STimeHandler timeHandler;
        private SBackgroundHandler backgroundHandler;
        private SSkyHandler skyHandler;
        private SCelestialBodyHandler celestialBodyHandler;
        private SCloudHandler cloudHandler;

        public override void Initialize()
        {
            this.timeHandler = new(this.SGameInstance);
            this.backgroundHandler = new(this.SGameInstance);
            this.skyHandler = new(this.SGameInstance);
            this.celestialBodyHandler = new(this.SGameInstance, this.timeHandler);
            this.cloudHandler = new(this.SGameInstance);

            this.timeHandler.Initialize();
            this.backgroundHandler.Initialize();
            this.skyHandler.Initialize();
            this.celestialBodyHandler.Initialize();
            this.cloudHandler.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.timeHandler.Update(gameTime);
            this.backgroundHandler.Update(gameTime);
            this.skyHandler.Update(gameTime);
            this.celestialBodyHandler.Update(gameTime);
            this.cloudHandler.Update(gameTime);
        }

        public void Reset()
        {
            this.cloudHandler.Reset();
        }
    }
}
