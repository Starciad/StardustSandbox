using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.UI.Elements
{
    internal sealed class Container : UIElement
    {
        internal Container()
        {
            this.CanDraw = true;
            this.CanUpdate = true;
        }

        internal override void Initialize()
        {
            base.Initialize();
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
