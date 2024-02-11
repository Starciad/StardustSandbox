using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Objects;

namespace PixelDust.Game.GUI
{
    public abstract class PGUISystem : PGameObject
    {
        public bool IsActive => this.isActive;
        public bool IsShowing => this.isShowing;
        public PGUILayout Layout => this.layout;

        private readonly PGUILayout layout;
        private bool isActive;
        private bool isShowing;

        public PGUISystem()
        {
            Activate();
            Close();

            this.layout = new();
        }

        protected override void OnAwake()
        {
            this.layout.Initialize(this.Game);
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            if (!this.isActive)
            {
                return;
            }

            this.layout.Update(gameTime);
        }
        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.isActive || !this.isShowing)
            {
                return;
            }

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

        public void Show()
        {
            this.isShowing = true;
        }

        public void Close()
        {
            this.isShowing = false;
        }
    }
}
