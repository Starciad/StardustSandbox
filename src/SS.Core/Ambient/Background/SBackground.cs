using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.Ambient.Background
{
    public sealed class SBackground(ISGame gameInstance, string identifier, Texture2D texture) : SGameObject(gameInstance)
    {
        public string Identifier => identifier;
        public bool IsAffectedByLighting { get; set; }

        private readonly Texture2D _texture = texture;

        private readonly List<SBackgroundLayer> layers = [];

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.layers.Count; i++)
            {
                this.layers[i].Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.layers.Count; i++)
            {
                this.layers[i].Draw(gameTime, spriteBatch);
            }
        }

        public void AddLayer(Point index, Vector2 parallaxFactor, Vector2 movementSpeed, bool lockX, bool lockY)
        {
            this.layers.Add(new(this.SGameInstance, this._texture, new Rectangle(new Point(index.X * SScreenConstants.DEFAULT_SCREEN_WIDTH, index.Y * SScreenConstants.DEFAULT_SCREEN_HEIGHT), new Point(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT)), parallaxFactor, movementSpeed, lockX, lockY));
        }
    }
}
