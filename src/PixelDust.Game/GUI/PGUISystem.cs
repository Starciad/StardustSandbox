using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Objects;

namespace PixelDust.Game.GUI
{
    public abstract class PGUISystem : PGameObject
    {
        public bool IsActive => this.isActive;
        public PGUILayout Layout => this.layout;

        private readonly PGUILayout layout;
        private bool isActive;

        public PGUISystem()
        {
            this.isActive = false;
            this.layout = new();
        }

        protected override void OnAwake()
        {
            this.layout.Initialize(this.Game);
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            this.layout.Update(gameTime);
        }
        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.layout.Draw(gameTime, spriteBatch);
        }

        public void Activate()
        {
            this.isActive = true;
        }
        public void Disable()
        {
            this.isActive = false;
        }
    }
}
