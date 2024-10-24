﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Interfaces.Elements;

namespace StardustSandbox.Game.Elements.Rendering
{
    public abstract class SElementRenderingMechanism
    {
        public virtual void Initialize(SElement element) { return; }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, ISElementContext context) { return; }
    }
}
