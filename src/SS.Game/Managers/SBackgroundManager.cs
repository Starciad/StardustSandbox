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
            this.background01.AddLayer(new Point(0, 0), new Vector2(0f), true, true);
            this.background01.AddLayer(new Point(0, 1), new Vector2(5f), false, true);
            this.background01.AddLayer(new Point(0, 2), new Vector2(8f), false, true);
            this.background01.AddLayer(new Point(0, 3), new Vector2(10f), false, true);
            this.background01.AddLayer(new Point(0, 4), new Vector2(15f), false, true);
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
