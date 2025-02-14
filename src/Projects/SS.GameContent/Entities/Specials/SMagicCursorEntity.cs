using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Components.Common.Entities;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.GameContent.Components.AI.Entities.Specials;

namespace StardustSandbox.GameContent.Entities.Specials
{
    internal sealed class SMagicCursorEntityDescriptor : SEntityDescriptor
    {
        internal SMagicCursorEntityDescriptor(string identifier) : base(identifier)
        {

        }

        public override SEntity CreateEntity(ISGame gameInstance)
        {
            return new SMagicCursorEntity(gameInstance, this.Identifier);
        }
    }

    internal sealed class SMagicCursorEntity : SEntity
    {
        private readonly Texture2D texture;

        private readonly SEntityTransformComponent transformComponent;
        private readonly SEntityGraphicsComponent graphicsComponent;
        private readonly SEntityRenderingComponent renderingComponent;

        internal SMagicCursorEntity(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.transformComponent = this.ComponentContainer.AddComponent(new SEntityTransformComponent(this.SGameInstance, this));
            this.graphicsComponent = this.ComponentContainer.AddComponent(new SEntityGraphicsComponent(this.SGameInstance, this));
            this.renderingComponent = this.ComponentContainer.AddComponent(new SEntityRenderingComponent(this.SGameInstance, this, this.transformComponent, this.graphicsComponent));
            _ = this.ComponentContainer.AddComponent(new SMagicCursorAIComponent(this.SGameInstance, this, this.transformComponent));

            // Graphics
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_cursor_1");
            this.graphicsComponent.SetTexture(this.texture);
            this.renderingComponent.ClipArea = new Rectangle(new(0), new(36));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public override void Reset()
        {
            return;
        }
    }
}