using StardustSandbox.Game.Background;
using StardustSandbox.Game.Objects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Game.Managers
{
    public sealed class SBackgroundManager(SGame gameInstance) : SGameObject(gameInstance)
    {
        private SBackground background01;

        public override void Initialize()
        {
            this.background01 = new(this.SGameInstance, this.SGameInstance.AssetDatabase.GetTexture("background_1"));
            this.background01.AddLayer(new Point(0, 0), new Vector2(30f), false, false);
        }

        public override void Update(GameTime gameTime)
        {
            this.background01.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.background01.Draw(gameTime, spriteBatch);
        }
    }
}
