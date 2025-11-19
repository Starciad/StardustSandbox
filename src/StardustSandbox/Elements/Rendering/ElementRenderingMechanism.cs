using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Elements.Rendering
{
    internal abstract class ElementRenderingMechanism
    {
        internal virtual void Initialize(Element element) { return; }
        internal virtual void Update(GameTime gameTime) { return; }
        internal virtual void Draw(SpriteBatch spriteBatch, ElementContext context) { return; }
    }
}
