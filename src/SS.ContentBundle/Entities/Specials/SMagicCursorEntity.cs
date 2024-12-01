using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.Components.Entities.AI;
using StardustSandbox.Core.Components.Common.Entities;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Entities.Specials
{
    internal sealed class SMagicCursorEntityDescriptor : SEntityDescriptor
    {
        internal SMagicCursorEntityDescriptor()
        {
            this.id = 0;
            this.associatedEntityType = typeof(SMagicCursorEntity);
        }

        public override SEntity CreateEntity(ISGame gameInstance)
        {
            return new SMagicCursorEntity(gameInstance);
        }
    }

    internal sealed class SMagicCursorEntity : SEntity
    {
        private readonly SEntityTransformComponent transformComponent;
        private readonly SEntityGraphicsComponent graphicsComponent;
        private readonly SEntityRenderingComponent renderingComponent;
        private readonly SMagicCursorEntityAIComponent aiComponent;

        private readonly Texture2D texture;

        public SMagicCursorEntity(ISGame gameInstance) : base(gameInstance)
        {
            this.transformComponent = this.ComponentContainer.AddComponent(new SEntityTransformComponent(this.SGameInstance, this));
            this.graphicsComponent = this.ComponentContainer.AddComponent(new SEntityGraphicsComponent(this.SGameInstance, this));
            this.renderingComponent = this.ComponentContainer.AddComponent(new SEntityRenderingComponent(this.SGameInstance, this, this.transformComponent, this.graphicsComponent));
            this.aiComponent = this.ComponentContainer.AddComponent(new SMagicCursorEntityAIComponent(this.SGameInstance, this, this.transformComponent));

            // Graphics
            this.texture = gameInstance.AssetDatabase.GetTexture("cursor_1");

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

        }
    }
}