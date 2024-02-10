using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Managers;

namespace PixelDust.Game.Scenes.Common
{
    public sealed class WorldScene : PScene
    {
        private SpriteFont font1;

        protected override void OnAwake()
        {
            this.font1 = this.Game.AssetsDatabase.GetFont("font_1");
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font1, PInputManager.DebugString, Vector2.Zero, Color.White);
        }
    }
}
