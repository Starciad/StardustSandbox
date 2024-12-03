using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.GUISystem.Elements
{
    public class SGUIContainerElement : SGUIElement
    {
        private readonly SGUILayout containerLayout;

        public SGUIContainerElement(ISGame gameInstance) : base(gameInstance)
        {
            this.containerLayout = new(gameInstance);

            this.IsVisible = true;
            this.ShouldUpdate = true;
        }

        public override void Initialize()
        {
            this.containerLayout.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.containerLayout.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.containerLayout.Draw(gameTime, spriteBatch);
        }

        public void Active()
        {
            this.containerLayout.IsActive = true;
        }

        public void Disable()
        {
            this.containerLayout.IsActive = false;
        }

        public void AddElement<T>(T value) where T : SGUIElement
        {
            this.containerLayout.AddElement(value);
        }
    }
}
