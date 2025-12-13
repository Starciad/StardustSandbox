using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Backgrounds;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Backgrounds;

namespace StardustSandbox.Scenario
{
    internal sealed class BackgroundHandler
    {
        internal bool IsAffectedByLighting => this.selectedBackground.IsAffectedByLighting;

        private Background selectedBackground;

        internal void Update(in GameTime gameTime)
        {
            this.selectedBackground.Update(gameTime);
        }

        internal void Draw(in SpriteBatch spriteBatch)
        {
            this.selectedBackground.Draw(spriteBatch);
        }

        internal void SetBackground(BackgroundIndex backgroundIndex)
        {
            this.selectedBackground = BackgroundDatabase.GetBackground(backgroundIndex);
        }
    }
}
