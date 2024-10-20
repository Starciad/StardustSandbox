using StardustSandbox.Game.Background;
using StardustSandbox.Game.Objects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Game.Managers
{
    public sealed class SBackgroundManager(SGame gameInstance) : SGameObject(gameInstance)
    {
        public Color SolidColor => this.solidColor;

        private Color solidColor;
        private SBackground selectedBackground;

        public override void Initialize()
        {
            SetColor(new Color(64, 116, 155));
            SetBackground(this.SGameInstance.BackgroundDatabase.GetBackgroundById("ocean_1"));
        }

        public override void Update(GameTime gameTime)
        {
            this.selectedBackground.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.selectedBackground.Draw(gameTime, spriteBatch);
        }

        public void SetColor(Color value)
        {
            this.solidColor = value;
        }

        public void SetBackground(SBackground background)
        {
            this.selectedBackground = background;
        }
    }
}
