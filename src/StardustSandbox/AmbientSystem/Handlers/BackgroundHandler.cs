using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.AmbientSystem.BackgroundSystem;

namespace StardustSandbox.AmbientSystem.Handlers
{
    internal sealed class BackgroundHandler
    {
        internal Color SolidColor { get; set; } = new(64, 116, 155);
        internal Background SelectedBackground => this.selectedBackground;

        private Background selectedBackground;

        internal void Update(GameTime gameTime)
        {
            this.selectedBackground.Update(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            this.selectedBackground.Draw(spriteBatch);
        }

        internal void SetBackground(Background background)
        {
            this.selectedBackground = background;
        }
    }
}
