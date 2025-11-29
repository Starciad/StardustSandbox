using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.UISystem.Elements;

namespace StardustSandbox.UISystem
{
    internal abstract class UI
    {
        internal UIIndex Index => this.index;
        internal bool IsActive => this.isActive;

        private bool isActive;

        private readonly Container root;
        private readonly UIIndex index;

        internal UI(UIIndex index)
        {
            this.index = index;

            this.root = new()
            {
                Alignment = CardinalDirection.Northwest,
                CanDraw = true,
                CanUpdate = true,
                Color = Color.Transparent,
                Margin = Vector2.Zero,
                Position = Vector2.Zero,
                Size = ScreenConstants.SCREEN_DIMENSIONS.ToVector2()
            };
        }

        internal virtual void Initialize()
        {
            OnBuild(this.root);
            this.root.Initialize();
        }

        internal virtual void Update(GameTime gameTime)
        {
            this.root.Update(gameTime);
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
        {
            this.root.Draw(spriteBatch);
        }

        internal void Open()
        {
            this.isActive = true;
            OnOpened();
        }

        internal void Close()
        {
            this.isActive = false;
            OnClosed();
        }

        protected abstract void OnBuild(Container root);
        protected virtual void OnOpened() { }
        protected virtual void OnClosed() { }

        public override string ToString()
        {
            return this.root.ToString();
        }
    }
}
