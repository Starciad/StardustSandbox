using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.BackgroundSystem;
using StardustSandbox.Databases;
using StardustSandbox.Enums.BackgroundSystem;

namespace StardustSandbox.AmbientSystem
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

        internal void SetBackground(BackgroundIndex backgroundIndex)
        {
            this.selectedBackground = BackgroundDatabase.GetBackground(backgroundIndex);
        }
    }
}
