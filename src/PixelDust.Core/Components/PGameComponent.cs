using Microsoft.Xna.Framework;

namespace PixelDust.Core.Components
{
    public abstract class PGameComponent
    {
        internal void Initialize() => OnInitialize();
        internal void LoadContent() => OnLoadContent();
        internal void Update(GameTime gameTime) => OnUpdate(gameTime);
        internal void UnloadContent() => OnUnloadContent();

        protected virtual void OnInitialize() { return; }
        protected virtual void OnLoadContent() { return; }
        protected virtual void OnUpdate(GameTime gameTime) { return; }
        protected virtual void OnUnloadContent() { return; }
    }
}
