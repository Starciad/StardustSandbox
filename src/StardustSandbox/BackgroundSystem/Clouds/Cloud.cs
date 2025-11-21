using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Interfaces.Collections;
using StardustSandbox.Randomness;

namespace StardustSandbox.BackgroundSystem.Clouds
{
    internal sealed class Cloud : IPoolableObject
    {
        internal Texture2D Texture => this.texture;
        internal Vector2 Position => this.position;

        private Texture2D texture;
        private Vector2 position;
        private float speed;
        private float opacity;

        public void Reset()
        {
            this.position = new Vector2(-(WorldConstants.GRID_SIZE * 5), SSRandom.Range(0, WorldConstants.GRID_SIZE * 10));
            this.speed = SSRandom.Range(10, 50);
            this.opacity = (float)((SSRandom.GetDouble() * 0.5) + 0.5);
        }

        internal Cloud()
        {
            Reset();
        }

        internal void Update(GameTime gameTime, SimulationSpeed simulationSpeed)
        {
            double multiplier = simulationSpeed switch
            {
                SimulationSpeed.Normal => 1.0,
                SimulationSpeed.Fast => 2.0,
                SimulationSpeed.VeryFast => 4.0,
                _ => 1.0,
            };

            this.position.X += (float)(this.speed * multiplier * gameTime.ElapsedGameTime.TotalSeconds);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, null, Color.White * this.opacity, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        internal void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }
    }
}
