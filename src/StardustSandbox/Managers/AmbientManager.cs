using Microsoft.Xna.Framework;

using StardustSandbox.AmbientSystem;
using StardustSandbox.Interfaces;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Managers
{
    internal sealed class AmbientManager : IResettable
    {
        internal BackgroundHandler BackgroundHandler => this.backgroundHandler;
        internal CelestialBodyHandler CelestialBodyHandler => this.celestialBodyHandler;
        internal CloudHandler CloudHandler => this.cloudHandler;
        internal SkyHandler SkyHandler => this.skyHandler;
        internal TimeHandler TimeHandler => this.timeHandler;

        private BackgroundHandler backgroundHandler;
        private CelestialBodyHandler celestialBodyHandler;
        private CloudHandler cloudHandler;
        private SkyHandler skyHandler;
        private TimeHandler timeHandler;

        public void Reset()
        {
            this.cloudHandler.Reset();
        }

        internal void Initialize(GameManager gameManager, World world)
        {
            this.backgroundHandler = new();
            this.cloudHandler = new(gameManager, world.Simulation);
            this.skyHandler = new();
            this.timeHandler = new(world.Time);
            this.celestialBodyHandler = new(this.timeHandler, world);
        }

        internal void Update(GameTime gameTime)
        {
            this.timeHandler.Update();
            this.backgroundHandler.Update(gameTime);
            this.celestialBodyHandler.Update();
            this.cloudHandler.Update(gameTime);
        }
    }
}
