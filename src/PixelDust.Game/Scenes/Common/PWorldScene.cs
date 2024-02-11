using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Managers;

namespace PixelDust.Game.Scenes.Common
{
    public sealed class PWorldScene : PScene
    {
        private SpriteFont font1;

        protected override void OnAwake()
        {
            this.font1 = this.Game.AssetDatabase.GetFont("font_1");
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.font1, PGameInputManager.DebugString, Vector2.Zero, Color.White);
        }
    }
}
