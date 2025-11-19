using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Managers;

using System.Collections.Generic;

namespace StardustSandbox.AmbientSystem.BackgroundSystem
{
    internal sealed class Background(string identifier, Texture2D texture)
    {
        internal string Identifier => identifier;
        internal bool IsAffectedByLighting { get; set; }

        private readonly Texture2D texture = texture;

        private readonly List<BackgroundLayer> layers = [];

        internal void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.layers.Count; i++)
            {
                this.layers[i].Update(gameTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.layers.Count; i++)
            {
                this.layers[i].Draw(spriteBatch);
            }
        }

        internal void AddLayer(CameraManager cameraManager, Point index, Vector2 parallaxFactor, Vector2 movementSpeed, bool lockX, bool lockY)
        {
            this.layers.Add(new(cameraManager, this.texture, new Rectangle(new Point(index.X * ScreenConstants.DEFAULT_SCREEN_WIDTH, index.Y * ScreenConstants.DEFAULT_SCREEN_HEIGHT), new Point(ScreenConstants.DEFAULT_SCREEN_WIDTH, ScreenConstants.DEFAULT_SCREEN_HEIGHT)), parallaxFactor, movementSpeed, lockX, lockY));
        }
    }
}
