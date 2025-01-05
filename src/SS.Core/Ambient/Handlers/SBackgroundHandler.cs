using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Ambient.Background;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Ambient.Handlers;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.Ambient.Handlers
{
    internal sealed class SBackgroundHandler(ISGame gameInstance) : SGameObject(gameInstance), ISBackgroundHandler
    {
        public Color SolidColor { get; set; } = new(64, 116, 155);
        public SBackground SelectedBackground => this.selectedBackground;

        private SBackground selectedBackground;

        public override void Update(GameTime gameTime)
        {
            this.selectedBackground.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.selectedBackground.Draw(gameTime, spriteBatch);
        }

        public void SetBackground(SBackground background)
        {
            this.selectedBackground = background;
        }
    }
}
