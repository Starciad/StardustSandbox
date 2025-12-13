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

        protected override void OnInitialize()
        {
            return;
        }

        protected override void OnUpdate(in GameTime gameTime)
        {
            return;
        }

        protected override void OnDraw(in SpriteBatch spriteBatch)
        {
            return;
        }
    }
}
