using Microsoft.Xna.Framework;

using StardustSandbox.Interfaces;
using StardustSandbox.Scenario;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Managers
{
    internal sealed class AmbientManager : IResettable
    {
        internal BackgroundHandler BackgroundHandler => this.backgroundHandler;
        internal CelestialBodyHandler CelestialBodyHandler => this.celestialBodyHandler;
        internal CloudHandler CloudHandler => this.cloudHandler;

        private BackgroundHandler backgroundHandler;
        private CelestialBodyHandler celestialBodyHandler;
        private CloudHandler cloudHandler;
        private TimeHandler timeHandler;

        public void Reset()
        {
            this.cloudHandler.Reset();
        }

        internal void Initialize(World world)
        {
            this.backgroundHandler = new();
            this.cloudHandler = new(world.Simulation);
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
